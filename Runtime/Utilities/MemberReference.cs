using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public class MemberReference
    {
        private readonly Delegate getter;
        private readonly Delegate setter;
        
        public MemberReference(string name, Type type, Delegate getter, Delegate setter)
        {
            this.Name = name;
            this.Type = type;
            this.getter = getter;
            this.setter = setter;
        }

        public string Name { get; }
        public Type Type { get; }

        public bool CanWrite
        {
            get { return setter != null; }
        }
        
        public object ReadValue(object instance)
        {
            return getter.DynamicInvoke(instance);
        }

        public void WriteValue(object instance, object value)
        {
            setter.DynamicInvoke(instance, value);
        }
    }
}