using System;

namespace Rehawk.UIFramework
{
    public delegate object MultiValueConvertFunctionDelegate(object[] values);
    public delegate object[] MultiValueConvertBackFunctionDelegate(object value);
    public delegate object MultiValueConvertFunctionDelegate<in T>(T[] values);
    
    public class FunctionCombiner : IValueCombiner
    {
        private readonly MultiValueConvertFunctionDelegate convertFunction;
        private readonly MultiValueConvertBackFunctionDelegate convertBackFunction;
        
        public FunctionCombiner(MultiValueConvertFunctionDelegate convertFunction)
        {
            this.convertFunction = convertFunction;
        }
        
        public FunctionCombiner(MultiValueConvertFunctionDelegate convertFunction, MultiValueConvertBackFunctionDelegate convertBackFunction)
        {
            this.convertFunction = convertFunction;
            this.convertBackFunction = convertBackFunction;
        }
        
        public object Combine(object[] values)
        {
            return convertFunction.Invoke(values);
        }

        public object[] Divide(object value)
        {
            if (convertBackFunction == null)
            {
                throw new Exception("Convert Back Function not set.");
            }
            
            return convertBackFunction.Invoke(value);
        }
    }
}