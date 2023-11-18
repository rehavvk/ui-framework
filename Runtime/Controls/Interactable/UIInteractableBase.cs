namespace Rehawk.UIFramework
{
    public abstract class UIInteractableBase : UIElementBase
    {
        public abstract bool Enabled { get; set; }
        public abstract bool IsInteractable { get; set; }
    }
}