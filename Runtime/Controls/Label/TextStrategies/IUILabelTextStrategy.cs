using System;

namespace Rehawk.UIFramework
{
    public interface IUILabelTextStrategy
    {
        public event Action<string> TextChanged;
        
        string GetText(UILabelBase label);
        bool SetText(UILabelBase label, string value);
    }
}