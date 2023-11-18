using System;

namespace Rehawk.UIFramework
{
    public abstract class UIPanelBase : UIElementBase
    {
        public virtual event Action<UIPanel> BecameVisible;
        public virtual event Action<UIPanel> BecameInvisible;
        
        public abstract UIPanel Parent { get; }
        
        public abstract void SetVisible(bool visible);
        public abstract void ToggleVisible();
    }
}