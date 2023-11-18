using System;
using System.Linq.Expressions;

namespace Rehawk.UIFramework
{
    public static class BindingToExtensions
    {
        public static Binding To<T>(this Binding binding, Expression<Func<T>> memberExpression, IValueConverter converter = null)
        {
            var bindingStrategy = new MemberBindingStrategy(() => binding.Parent, MemberPath.Get(memberExpression));
            bindingStrategy.SetConverter(converter);
            
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            
            return binding;
        }

        public static Binding To<T>(this Binding binding, Expression<Func<T>> memberExpression, ValueConvertFunctionDelegate<T> converterFunction)
        {
            var converter = FunctionConverter.CreateTyped(converterFunction);
            
            var bindingStrategy = new MemberBindingStrategy(() => binding.Parent, MemberPath.Get(memberExpression));
            bindingStrategy.SetConverter(converter);
            
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            
            return binding;
        }

        public static Binding ToProperty(this Binding binding, Func<object> getContextFunction, string propertyName, IValueConverter converter = null) 
        {
            var bindingStrategy = new ContextPropertyBindingStrategy(getContextFunction, propertyName);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }
        
        public static Binding ToProperty(this Binding binding, Func<object> getContextFunction, string propertyName, ValueConvertFunctionDelegate converterFunction) 
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new ContextPropertyBindingStrategy(getContextFunction, propertyName);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }
        
        public static Binding ToContext(this Binding binding, Func<UIContextControlBase> getControlFunction, IValueConverter converter = null)
        {
            var bindingStrategy = new ContextBindingStrategy(getControlFunction);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        public static Binding ToContext(this Binding binding, Func<UIContextControlBase> getControlFunction, ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new ContextBindingStrategy(getControlFunction);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction, IValueConverter converter = null)
        {
            return binding.ToCallback(getFunction, _ => {}, converter);
        }
        
        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction, ValueConvertFunctionDelegate converterFunction)
        {
            return binding.ToCallback(getFunction, _ => {}, converterFunction);
        }
        
        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction, Action<T> setCallback, IValueConverter converter = null)
        {
            var bindingStrategy = new CallbackBindingStrategy<T>(getFunction, setCallback);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }
        
        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction, Action<T> setCallback, ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new CallbackBindingStrategy<T>(getFunction, setCallback);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }
        
        public static Binding ToValue<T>(this Binding binding, T value, IValueConverter converter = null)
        {
            var bindingStrategy = new StaticValueBindingStrategy(value);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }
        
        public static Binding ToValue<T>(this Binding binding, T value, ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new StaticValueBindingStrategy(value);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }
        
        public static Binding ToCustom(this Binding binding, IBindingStrategy bindingStrategy)
        {
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }
    }
}