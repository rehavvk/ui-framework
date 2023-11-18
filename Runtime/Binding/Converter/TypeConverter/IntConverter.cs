namespace Rehawk.UIFramework
{
    public class IntConverter : IValueConverter
    {
        public object Convert(object value)
        {
            string inputStr = value?.ToString();
                
            if (int.TryParse(inputStr, out int result))
            {
                return result;
            }

            return 0;
        }

        public object ConvertBack(object value)
        {
            return value;
        }
    }
}