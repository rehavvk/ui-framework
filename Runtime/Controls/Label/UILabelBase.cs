namespace Rehawk.UIFramework
{
    public abstract class UILabelBase : UIGraphicBase
    {
        public abstract string Text { get; set; }

        public abstract void SetStrategy(IUILabelTextStrategy strategy);
    }
}