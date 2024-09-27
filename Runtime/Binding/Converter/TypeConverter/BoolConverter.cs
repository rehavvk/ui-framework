namespace Rehawk.UIFramework
{
    public class BoolConverter : IValueConverter
    {
        private readonly bool invert;
        
        public BoolConverter(bool invert = false)
        {
            this.invert = invert;
        }

        public virtual object Convert(object value)
        {
            bool result = value != null;
            
            if (value is bool boolValue)
            {
                result = boolValue;
            }
            else if (value != null && value.IsNumber())
            {
                string inputStr = value?.ToString();
                decimal decimalValue = decimal.Parse(inputStr);
                result = decimalValue > 0;
            }
            else if (value is string stringValue)
            {
                result = !string.IsNullOrEmpty(stringValue);
            }

            if (invert)
            {
                return !result;
            }
            
            return result;
        }

        public virtual object ConvertBack(object value)
        {
            if (invert && value is bool boolValue)
            {
                return !boolValue;
            }
                
            return value;
        }
    }
}