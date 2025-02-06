using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public class BindedMember
    {
        private readonly Func<object> getOriginFunction;
        private readonly string memberPath;
        private readonly string[] memberPathSplit;
        private readonly MemberReference[] memberReferences;
        private readonly object[] memberValues;

        private string memberName;
        
        private object origin;
        private object context;
        private object value;
        
        public event Action GotDirty;

        public BindedMember(Func<object> getOriginFunction, string memberPath)
        {
            this.getOriginFunction = getOriginFunction;
            this.memberPath = memberPath;
            
            memberPathSplit = memberPath.Split('.');

            memberReferences = new MemberReference[memberPathSplit.Length];
            memberValues = new object[memberPathSplit.Length - 1];
        }

        public void Evaluate()
        {
            UnLinkFromEvents();
            
            origin = getOriginFunction?.Invoke();
            
            EvaluateMemberPath();
            EvaluateContext();
            
            value = Get();

            LinkToEvents();
        }
        
        public void Release()
        {
            UnLinkFromEvents();
        }

        private void EvaluateMemberPath()
        {
            if (origin != null && !string.IsNullOrEmpty(memberPath))
            {
                Type type = origin.GetType();
                
                for (int i = 0; i < memberPathSplit.Length; i++)
                {
                    memberName = memberPathSplit[i];

                    MemberReference memberReference = MemberReferenceCache.Get(type, memberName);
                    
                    if (memberReference == null)
                    {
                        Debug.LogError($"Part of path to member couldn't be found. [memberName={memberName}, path={memberPath}]");
                        break;
                    }

                    memberReferences[i] = memberReference;
                    
                    type = memberReference.Type;
                }
            }
        }

        private void EvaluateContext()
        {
            context = origin;

            for (int i = 0; i < memberReferences.Length - 1; i++)
            {
                if (context != null)
                {
                    context = memberReferences[i].ReadValue(context);
                    memberValues[i] = context;
                }
            }
        }

        private void ReevaluateContext()
        {
            UnLinkFromEvents();

            EvaluateContext();
            
            LinkToEvents();
            
            GotDirty?.Invoke();
        }

        public object Get()
        {
            object result = null;

            if (context != null && memberReferences != null && memberReferences.Length > 0)
            {
                MemberReference memberReference = memberReferences[^1];
                result = memberReference.ReadValue(context);
            }
            
            return result;
        }

        public void Set(object value)
        {
            if (context != null && memberReferences != null && memberReferences.Length > 0)
            {
                MemberReference memberReference = memberReferences[^1];
                if (memberReference.CanWrite)
                {
                    memberReference.WriteValue(context, value);
                }
                else
                {
                    Debug.LogError($"Member is not writeable. [memberName={memberName}, path={memberPath}]");
                }
            }
        }
        
        private void LinkToEvents()
        {
            if (origin is INotifyPropertyChanged originNotifyPropertyChanged)
            {
                originNotifyPropertyChanged.PropertyChanged += OnOriginPropertyChanged;
            }
            
            if (memberValues != null)
            {
                for (int i = 0; i < memberValues.Length; i++)
                {
                    object memberValue = memberValues[i];
                
                    if (memberValue is INotifyPropertyChanged memberValueNotifyPropertyChanged)
                    {
                        memberValueNotifyPropertyChanged.PropertyChanged += OnMemberValuePropertyChanged;
                    }
                }
            }

            if (value is INotifyCollectionChanged valueNotifyCollectionChanged)
            {
                valueNotifyCollectionChanged.CollectionChanged += OnValueCollectionChanged;
            }
            else if (value is INotifyPropertyChanged valueNotifyPropertyChanged)
            {
                valueNotifyPropertyChanged.PropertyChanged += OnValuePropertyChanged;
            }
            
            if (context is INotifyCollectionChanged contextNotifyCollectionChanged)
            {
                contextNotifyCollectionChanged.CollectionChanged += OnContextCollectionChanged;
            }
            else if (context is INotifyPropertyChanged contextNotifyPropertyChanged)
            {
                contextNotifyPropertyChanged.PropertyChanged += OnContextPropertyChanged;
            }
        }

        private void UnLinkFromEvents()
        {
            if (origin is INotifyPropertyChanged originNotifyPropertyChanged)
            {
                originNotifyPropertyChanged.PropertyChanged -= OnOriginPropertyChanged;
            }

            if (memberValues != null)
            {
                for (int i = 0; i < memberValues.Length; i++)
                {
                    object memberValue = memberValues[i];
                
                    if (memberValue is INotifyPropertyChanged memberValueNotifyPropertyChanged)
                    {
                        memberValueNotifyPropertyChanged.PropertyChanged -= OnMemberValuePropertyChanged;
                    }
                }
            }
            
            if (value is INotifyCollectionChanged valueNotifyCollectionChanged)
            {
                valueNotifyCollectionChanged.CollectionChanged -= OnValueCollectionChanged;
            }
            else if (value is INotifyPropertyChanged valueNotifyPropertyChanged)
            {
                valueNotifyPropertyChanged.PropertyChanged -= OnValuePropertyChanged;
            }
            
            if (context is INotifyCollectionChanged contextNotifyCollectionChanged)
            {
                contextNotifyCollectionChanged.CollectionChanged -= OnContextCollectionChanged;
            }
            else if (context is INotifyPropertyChanged contextNotifyPropertyChanged)
            {
                contextNotifyPropertyChanged.PropertyChanged -= OnContextPropertyChanged;
            }
        }

        private void OnOriginPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (memberReferences != null && memberReferences[0].Name == e.PropertyName)
            {
                ReevaluateContext();
            }
        }
        
        private void OnMemberValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (memberValues != null)
            {
                int memberIndex = Array.IndexOf(memberValues, sender);

                if (memberReferences[memberIndex + 1].Name == e.PropertyName)
                {
                    ReevaluateContext();
                }
            }
        }
        
        private void OnValueCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GotDirty?.Invoke();
        }

        private void OnValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Caused a second invocation of GotDirty after OnMemberValuePropertyChanged.

            //GotDirty?.Invoke();
        }
        
        private void OnContextCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GotDirty?.Invoke();
        }

        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Caused a second invocation of GotDirty after OnMemberValuePropertyChanged.
            
            // if (string.IsNullOrEmpty(memberName) || e.PropertyName == memberName)
            // {
            //     GotDirty?.Invoke();
            // }
        }
    }
}