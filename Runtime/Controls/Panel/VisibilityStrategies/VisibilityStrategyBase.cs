using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public abstract class VisibilityStrategyBase
    {
        public abstract bool IsVisible { get; }
        public abstract void SetVisible(UIPanelBase panel, bool visible, Action doneCallback);
    }
}