using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class CanvasGroupVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private CanvasGroup group;
        
        [SerializeField] private CanvasGroupSettings visibleSettings = new CanvasGroupSettings { Alpha = 1, IsInteractable = true, IsBlockingRaycasts = true };
        [SerializeField] private CanvasGroupSettings invisibleSettings = new CanvasGroupSettings { Alpha = 0, IsInteractable = false, IsBlockingRaycasts = false };
        
        private bool isVisible;
        
        public override bool IsVisible => isVisible;

        public override void SetVisible(UIPanelBase panel, bool visible, Action doneCallback)
        {
            isVisible = visible;
            group.alpha = isVisible ? visibleSettings.Alpha : invisibleSettings.Alpha;
            group.interactable = isVisible ? visibleSettings.IsInteractable : invisibleSettings.IsInteractable;
            group.blocksRaycasts = isVisible ? visibleSettings.IsBlockingRaycasts : invisibleSettings.IsBlockingRaycasts;
            doneCallback?.Invoke();
        }

        [Serializable]
        public struct CanvasGroupSettings
        {
            public float Alpha;
            public bool IsInteractable;
            public bool IsBlockingRaycasts;
        }
    }
}