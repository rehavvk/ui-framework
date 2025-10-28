using System;

namespace Rehawk.UIFramework
{
    public delegate object ValueConvertFunctionDelegate(object value);
    public delegate object ValueConvertFunctionDelegate<in T>(T value);

    public class FunctionConverter : IValueConverter
    {
        private readonly ValueConvertFunctionDelegate convertFunction;
        private readonly ValueConvertFunctionDelegate convertBackFunction;
        
        public FunctionConverter(ValueConvertFunctionDelegate convertFunction)
        {
            this.convertFunction = convertFunction;
        }
        
        public FunctionConverter(ValueConvertFunctionDelegate convertFunction, ValueConvertFunctionDelegate convertBackFunction)
        {
            this.convertFunction = convertFunction;
            this.convertBackFunction = convertBackFunction;
        }
        
        public object Convert(object value)
        {
            return convertFunction.Invoke(value);
        }

        public object ConvertBack(object value)
        {
            if (convertBackFunction == null)
            {
                throw new Exception("Convert Back Function not set.");
            }

            return convertBackFunction.Invoke(value);
        }

        public static FunctionConverter CreateTyped<T>(ValueConvertFunctionDelegate<T> convertFunction, ValueConvertFunctionDelegate convertBackFunction = null)
        {
            return new FunctionConverter(value =>
            {
                if (value != null)
                {
                    return convertFunction.Invoke((T)value);
                }
                else
                {
                    return convertFunction.Invoke(default);
                }
            }, convertBackFunction);
        }
    }
}