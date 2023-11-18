using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Rehawk.UIFramework
{
    public static class ReflectionUtility
    {
        private const BindingFlags FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        
        public static IEnumerable<MemberInfo> GetAllMembers(Type type)
        {
            if (type == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            return type.GetMembers(FLAGS)
                       .Concat(GetAllMembers(type.BaseType));
        }   
        
        public static IEnumerable<MemberInfo> GetMember(Type type, string memberName)
        {
            if (type == null)
            {
                return Enumerable.Empty<FieldInfo>();
            }

            return type.GetMember(memberName, FLAGS)
                       .Concat(GetMember(type.BaseType, memberName));
        }   
    }
}