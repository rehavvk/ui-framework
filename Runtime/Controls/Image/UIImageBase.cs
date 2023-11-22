using UnityEngine;

namespace Rehawk.UIFramework
{
    public abstract class UIImageBase : UIGraphicBase
    {
        public abstract Sprite Sprite { get; set; }

        public abstract float FillAmount { get; set; }
    }
}