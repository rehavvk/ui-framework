using System;

namespace Rehawk.UIFramework
{
    public class DateTimeToStringConverter : IValueConverter
    {
        private readonly string format;
        
        public DateTimeToStringConverter(string format = null)
        {
            this.format = format;
        }
        
        public object Convert(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString(format);
            }
                
            return value;
        }

        public object ConvertBack(object value)
        {
            return value;
        }
    }
}