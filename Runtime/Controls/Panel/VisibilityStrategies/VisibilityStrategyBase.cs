using System;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Serves as a base class for defining visibility strategies for UI panels.
    /// Visibility strategies determine how the visibility of a panel is managed,
    /// including any animations or transitions required during visibility changes.
    /// </summary>
    [Serializable]
    public abstract class VisibilityStrategyBase
    {
        public abstract bool IsVisible { get; }
        
        public abstract void SetVisible(UIPanelBase panel, bool visible, bool instant, Action doneCallback);
    }
}