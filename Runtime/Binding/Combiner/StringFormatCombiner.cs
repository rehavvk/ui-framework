using System;

namespace Rehawk.UIFramework
{
    public class StringFormatCombiner : IValueCombiner
    {
        private readonly string format;
        
        public StringFormatCombiner(string format)
        {
            this.format = format;
        }
        
        public object Combine(object[] values)
        {
            return string.Format(format, values);
        }

        public object[] Divide(object value)
        {
            throw new NotSupportedException("Back conversion of multiple values formatted as string is not supported.");
        }
    }
}