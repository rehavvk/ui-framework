using UnityEngine;

namespace Rehawk.UIFramework
{
    public abstract class UIGraphicBase : UIElementBase
    {
        public abstract bool Enabled { get; set; }
        public abstract Material Material { get; set; }
        public abstract Color Color { get; set; }
    }
}