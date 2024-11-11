using System.Collections;
using System.Collections.Generic;

namespace Rehawk.UIFramework
{
    public class ListConverter : IValueConverter
    {
        private readonly List<object> list = new List<object>(1);
        
        public virtual object Convert(object value)
        {
            list.Clear();

            if (value is IEnumerable enumerable)
            {
                return enumerable;
            }
            else if (value != null)
            {
                list.Add(value);
            }

            return list;
        }

        public virtual object ConvertBack(object value)
        {
            if (value == list)
            {
                return list[0];
            }

            return value;
        }
    }
}