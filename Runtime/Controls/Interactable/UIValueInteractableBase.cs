namespace Rehawk.UIFramework
{
    public abstract class UIValueInteractableBase : UIInteractableBase
    {
        public abstract object BoxedValue { get; set; }
        public abstract ICommand ChangedCommand { get; set; }
    }
}