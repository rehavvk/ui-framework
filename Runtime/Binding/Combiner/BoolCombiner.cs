using System;

namespace Rehawk.UIFramework
{
    public class BoolCombiner : IValueCombiner
    {
        private readonly Mode mode;
        private readonly BoolConverter boolConverter = new BoolConverter();

        public BoolCombiner(Mode mode)
        {
            this.mode = mode;
        }
        
        public object Combine(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var boolValue = (bool) boolConverter.Convert(values[i]);
                
                switch (mode)
                {
                    case Mode.And:
                        if (!boolValue)
                            return false;
                        break;
                    case Mode.Or:
                        if (boolValue)
                            return true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }            
            }

            return true;
        }

        public object[] Divide(object value)
        {
            throw new NotSupportedException("Back conversion of multiple values is not supported.");
        }

        public enum Mode
        {
            And = 0,
            Or = 1
        }
    }
}