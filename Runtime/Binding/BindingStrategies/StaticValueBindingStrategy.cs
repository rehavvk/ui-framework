using System;

namespace Rehawk.UIFramework
{
    public class StaticValueBindingStrategy : IBindingStrategy
    {
        private object value;
        private IValueConverter converter;

        public event Action GotDirty;

        public StaticValueBindingStrategy(object value)
        {
            this.value = value;
        }

        public void SetConverter(IValueConverter converter)
        {
            this.converter = converter;
        }

        public void Evaluate() { }

        public void Release() { }
        
        public object Get()
        {
            object value = this.value;
            
            if (converter != default)
            {
                value = converter.Convert(value);
            }

            return value;
        }

        public void Set(object value)
        {
            if (converter != default)
            {
                value = converter.ConvertBack(value);
            }
            
            if (this.value != value)
            {
                this.value = value;
                GotDirty?.Invoke();
            }
        }
    }
}