using System;

namespace Rehawk.UIFramework
{
    public interface IUILabelTextStrategy
    {
        public event Action<string> TextChanged;
        
        string GetText(UILabel label);
        bool SetText(UILabel label, string value);
    }
}