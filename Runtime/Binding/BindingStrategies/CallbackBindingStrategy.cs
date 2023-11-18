using System;

namespace Rehawk.UIFramework
{
    public class CallbackBindingStrategy<T> : IBindingStrategy
    {
        private readonly Func<T> getFunction;
        private readonly Action<T> setCallback;
        
        private IValueConverter converter;

        // Has no implementation for that.
        public event Action GotDirty;

        public CallbackBindingStrategy(Func<T> getFunction, Action<T> setCallback)
        {
            this.getFunction = getFunction;
            this.setCallback = setCallback;
        }

        public void SetConverter(IValueConverter converter)
        {
            this.converter = converter;
        }

        public void Evaluate() { }

        public void Release() { }
        
        public object Get()
        {
            object result = null;

            if (getFunction != null)
            {
                result = getFunction.Invoke();
            }

            if (converter != default)
            {
                result = converter.Convert(result);
            }

            return result;
        }

        public void Set(object value)
        {
            if (converter != default)
            {
                value = converter.ConvertBack(value);
            }

            if (value != null)
            {
                setCallback.Invoke((T) value);
            }
            else
            {
                setCallback.Invoke(default);
            }
        }
    }
}