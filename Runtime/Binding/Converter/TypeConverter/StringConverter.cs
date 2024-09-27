using System;
using System.Globalization;

namespace Rehawk.UIFramework
{
    public class StringConverter : IValueConverter
    {
        private readonly string format;
        private readonly CultureInfo cultureInfo;
        
        public StringConverter(string format = null, CultureInfo cultureInfo = null)
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
            if (string.IsNullOrEmpty(format))
            {
                return value?.ToString();
            }
            
            if (value != null)
            {
                if (value.IsNumber() && value is IFormattable formattableValue)
                {
                    return formattableValue.ToString(format, CultureInfo);
                }

                return string.Format(format, value);
            }

            return null;
        }

        public object ConvertBack(object value)
        {
            return value;
        }
    }
}