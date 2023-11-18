using System;

namespace Rehawk.UIFramework
{
    public class DefaultUILabelTextStrategy : IUILabelTextStrategy
    {
        private string text;

        public event Action<string> TextChanged;

        public string GetText(UILabel label)
        {
            return text;
        }

        public bool SetText(UILabel label, string value)
        {
            if (text != value)
            {
                text = value;
                
                TextChanged?.Invoke(value);
                
                return true;
            }

            return false;
        }
    }
}