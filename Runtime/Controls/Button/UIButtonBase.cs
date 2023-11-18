namespace Rehawk.UIFramework
{
    public abstract class UIButtonBase : UIInteractableBase
    {
        public abstract ICommand ClickCommand { get; set; }
        public abstract ICommand HoverBeginCommand { get; set; }
        public abstract ICommand HoverEndCommand { get; set; }
    }
}