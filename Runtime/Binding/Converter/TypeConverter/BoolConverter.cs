namespace Rehawk.UIFramework
{
    public class BoolConverter : IValueConverter
    {
        public virtual object Convert(object value)
        {
            if (value is bool boolValue)
            {
                return boolValue;
            }

            if (value is int intValue)
            {
                return intValue > 0;
            }
                
            if (value is float floatValue)
            {
                return floatValue > 0;
            }
                
            if (value is string stringValue)
            {
                return !string.IsNullOrEmpty(stringValue);
            }
                
            return value != null;
        }

        public virtual object ConvertBack(object value)
        {
            return value;
        }
    }
}