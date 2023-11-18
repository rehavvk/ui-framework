using UnityEngine;

namespace Rehawk.UIFramework
{
    public class HexCodeToColorConverter : IValueConverter
    {
        public object Convert(object value)
        {
            if (value != null && ColorUtility.TryParseHtmlString(value.ToString(), out Color color))
            {
                return color;
            }

            return Color.white;
        }

        public object ConvertBack(object value)
        {
            if (value is Color color)
            {
                return ColorUtility.ToHtmlStringRGBA(color);
            }
            
            return ColorUtility.ToHtmlStringRGBA(Color.white);
        }
    }
}