namespace Rehawk.UIFramework
{
    public class FloatConverter : IValueConverter
    {
        public object Convert(object value)
        {
            string inputStr = value?.ToString();
                
            if (float.TryParse(inputStr, out float result))
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