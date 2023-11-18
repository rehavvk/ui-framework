using System;

namespace Rehawk.UIFramework
{
    public class ContextBindingStrategy : IBindingStrategy
    {
        private readonly Func<UIContextControlBase> getControlFunction;

        private IValueConverter converter;
        
        private UIContextControlBase control;

        public event Action GotDirty;

        public ContextBindingStrategy(Func<UIContextControlBase> getControlFunction)
        {
            this.getControlFunction = getControlFunction;
        }

        public void SetConverter(IValueConverter converter)
        {
            this.converter = converter;
        }

        public void Evaluate()
        {
            UnLinkFromEvents();

            control = getControlFunction.Invoke();
            
            LinkToEvents();
        }

        public void Release()
        {
            UnLinkFromEvents();
        }

        public object Get()
        {
            object value = null;

            if (control != null)
            {
                value = control.RawContext;
            }
            
            if (converter != default)
            {
                value = converter.Convert(value);
            }

            return value;
        }

        public void Set(object value)
        {
            if (control != null)
            {
                if (converter != default)
                {
                    value = converter.ConvertBack(value);
                }

                control.SetContext(value);
            }
        }
        
        private void LinkToEvents()
        {
            if (control != null)
            {
                control.ContextChanged += OnControlContextChanged;
            }
        }

        private void UnLinkFromEvents()
        {
            if (control != null)
            {
                control.ContextChanged -= OnControlContextChanged;
            }
        }
        
        private void OnControlContextChanged()
        {
            GotDirty?.Invoke();
        }
    }
}