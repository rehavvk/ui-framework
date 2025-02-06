using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public static class MemberReferenceCache
    {
        private const BindingFlags FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        private static readonly Dictionary<string, MemberReference> cache = 
            new Dictionary<string, MemberReference>();

        public static MemberReference Get(Type type, string memberName)
        {
            if (type == null)
                return null;

            string cacheKey = $"{type.FullName}.{memberName}";

            if (cache.TryGetValue(cacheKey, out MemberReference memberRef))
            {
                return memberRef;
            }

            MemberInfo memberInfo = FindMemberInfoRecursive(type, memberName);

            if (memberInfo == null)
                return null;

            Type memberType = memberInfo.GetUnderlyingType();
            
            ParameterExpression instanceExpression = Expression.Parameter(type, "instance");
            
            MemberExpression memberExpression;
            bool canWrite = false;
            
            if (memberInfo is PropertyInfo propertyInfo)
            {
                memberExpression = Expression.Property(instanceExpression, propertyInfo);
                canWrite = propertyInfo.CanWrite;
            }
            else if (memberInfo is FieldInfo fieldInfo)
            {
                memberExpression = Expression.Field(instanceExpression, fieldInfo);
                canWrite = !fieldInfo.IsInitOnly && !fieldInfo.IsLiteral;
            }
            else
            {
                Debug.LogError($"Member type is not supported. [memberName={memberName}, memberType={memberInfo.MemberType}]");
                return null;
            }

            Delegate setter = null;
            
            if (canWrite)
            {
                ParameterExpression valueParam = Expression.Parameter(memberType, "value");
                BinaryExpression assign = Expression.Assign(memberExpression, valueParam);
                setter = Expression.Lambda(assign, instanceExpression, valueParam)
                                            .Compile();
            }
            
            Delegate getter = Expression.Lambda(memberExpression, instanceExpression)
                                        .Compile();

            memberRef = new MemberReference(memberInfo.Name, memberType, getter, setter);
            
            cache.Add(cacheKey, memberRef);
            
            return memberRef;
        }
        
        private static MemberInfo FindMemberInfoRecursive(Type type, string memberName)
        {
            while (type != null)
            {
                MemberInfo[] memberInfos = type.GetMember(memberName, FLAGS);
                
                if (memberInfos.Length > 0)
                {
                    return memberInfos[0];
                }
                
                type = type.BaseType;
            }

            return null;
        }
    }
}