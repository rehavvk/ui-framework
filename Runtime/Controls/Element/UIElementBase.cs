using UnityEngine;

namespace Rehawk.UIFramework
{
    public abstract class UIElementBase : UIControlBase
    {
        public abstract bool IsVisible { get; set; }
    }
}