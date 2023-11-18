using System.Collections.Generic;

namespace Rehawk.UIFramework
{
    public class MultiConverter : IValueConverter
    {
        private readonly List<IValueConverter> converters = new List<IValueConverter>();

        public void AddConverter(IValueConverter converter)
        {
            converters.Add(converter);
        }
        
        public object Convert(object value)
        {
            for (int i = 0; i < converters.Count; i++)
            {
                value = converters[i].Convert(value);
            }

            return value;
        }

        public object ConvertBack(object value)
        {
            for (int i = 0; i < converters.Count; i++)
            {
                value = converters[i].ConvertBack(value);
            }

            return value;
        }
    }
}