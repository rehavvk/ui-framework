namespace Rehawk.UIFramework
{
    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value)
        {
            string inputStr = value?.ToString();
                
            if (double.TryParse(inputStr, out double result))
            {
                return result;
            }

            return 0f;
        }

        public object ConvertBack(object value)
        {
            return value;
        }
    }
}