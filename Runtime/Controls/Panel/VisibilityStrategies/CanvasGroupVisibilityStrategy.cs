using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class CanvasGroupVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float visibleAlpha = 1;
        [SerializeField] private float inVisibleAlpha = 0;

        private bool isVisible;
        
        public override bool IsVisible
        {
            get { return isVisible; }
        }

        public override void SetVisible(bool visible, Action callback)
        {
            isVisible = visible;
            group.alpha = isVisible ? visibleAlpha : inVisibleAlpha;
            callback?.Invoke();
        }
    }
}