namespace Rehawk.UIFramework
{
    public abstract class UISliderBase : UIValueInteractableBase
    {
        public abstract float Value { get; set; }
        public abstract float NormalizedValue { get; set; }
        public abstract float MinValue { get; set; }
        public abstract float MaxValue { get; set; }
    }
}