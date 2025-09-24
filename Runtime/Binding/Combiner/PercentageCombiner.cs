using System;

namespace Rehawk.UIFramework
{
    public class PercentageCombiner : IValueCombiner
    {
        public object Combine(object[] values)
        {
            if (values.Length < 2 || values[0] == null || values[1] == null)
                return 0;

            if (float.TryParse(values[0].ToString(), out float part) &&
                float.TryParse(values[1].ToString(), out float whole) &&
                whole != 0)
            {
                return (part / whole);
            }

            return 0;
        }

        public object[] Divide(object value)
        {
            throw new NotSupportedException("Back conversion of percentage is not supported.");
        }
    }
}