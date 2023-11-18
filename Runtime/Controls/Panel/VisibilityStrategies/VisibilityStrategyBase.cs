using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public abstract class VisibilityStrategyBase
    {
        public abstract bool IsVisible { get; }
        public abstract void SetVisible(bool visible, Action callback);
    }
}