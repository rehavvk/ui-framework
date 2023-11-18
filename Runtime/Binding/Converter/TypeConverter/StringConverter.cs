namespace Rehawk.UIFramework
{
    public class StringConverter : IValueConverter
    {
        private readonly string format;
        
        public StringConverter(string format = null)
        {
            this.format = format;
        }
        
        public object Convert(object value)
        {
            if (string.IsNullOrEmpty(format))
            {
                return value?.ToString();
            }
            
            return string.Format(format, value);
        }

        public object ConvertBack(object value)
        {
            return value;
        }
    }
}