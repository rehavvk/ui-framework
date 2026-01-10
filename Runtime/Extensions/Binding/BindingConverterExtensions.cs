using System;

namespace Rehawk.UIFramework
{
    public static class BindingConverterExtensions
    {
        public static Binding ConvertByFunction(this Binding binding, ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);
            
            MultiConverterHelper.AddConverter(binding, converter);

            return binding;
        }

        public static Binding ConvertByFunction<T>(this Binding binding, ValueConvertFunctionDelegate<T> converterFunction) 
        {
            var converter = FunctionConverter.CreateTyped(converterFunction);
            
            MultiConverterHelper.AddConverter(binding, converter);

            return binding;
        }

        public static Binding ConvertTo<T>(this Binding binding)
        {
            return binding.ConvertByFunction(input =>
            {
                if (input != null)
                {
                    return (T) input;
                }

                return default(T);
            });
        }

        public static Binding ConvertToBool(this Binding binding, bool invert = false)
        {
            return binding.ConvertBy(new BoolConverter(invert));
        }

        public static Binding ConvertToInvertedBool(this Binding binding)
        {
            return binding.ConvertToBool(true);
        }

        public static Binding ConvertToInt(this Binding binding)
        {
            return binding.ConvertBy(new IntConverter());
        }

        public static Binding ConvertToFloat(this Binding binding)
        {
            return binding.ConvertBy(new FloatConverter());
        }

        public static Binding ConvertToDouble(this Binding binding)
        {
            return binding.ConvertBy(new DoubleConverter());
        }

        public static Binding ConvertToString(this Binding binding)
        {
            return binding.ConvertBy(new StringConverter());
        }

        public static Binding ConvertToString(this Binding binding, string format)
        {
            return binding.ConvertBy(new StringConverter(format));
        }

        public static Binding ConvertToDateTimeString(this Binding binding, string format)
        {
            return binding.ConvertBy(new DateTimeToStringConverter(format));
        }

        public static Binding ConvertBy(this Binding binding, IValueConverter converter) 
        {
            MultiConverterHelper.AddConverter(binding, converter);
            
            return binding;
        }
    }
}