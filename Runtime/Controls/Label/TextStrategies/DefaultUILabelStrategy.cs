using System;

namespace Rehawk.UIFramework
{
    public class DefaultTextStrategy : IUILabelTextStrategy
    {
        private string text;

        public event Action<string> TextChanged;

        public string GetText(UILabelBase label)
        {
            return text;
        }

        public bool SetText(UILabelBase label, string value)
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