using System;
using System.Globalization;
using Rehawk.UIFramework;

namespace Rehawk.UIFramework
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        private readonly string format;
        private readonly CultureInfo cultureInfo;
        
        public TimeSpanToStringConverter(string format = null, CultureInfo cultureInfo = null)
        {
            this.format = format;
            this.cultureInfo = cultureInfo;
        }
        
        private CultureInfo CultureInfo
        {
            get { return cultureInfo != null ? cultureInfo : CultureInfo.CurrentUICulture; }
        }

        public object Convert(object value)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(format, CultureInfo);
            }
                
            return value;
        }

        public object ConvertBack(object value)
        {
            return value;
        }
    }
}