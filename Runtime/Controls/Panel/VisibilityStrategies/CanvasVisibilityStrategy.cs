using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class CanvasVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private Canvas canvas;

        public override bool IsVisible => canvas.enabled;

        public override void SetVisible(UIPanelBase panel, bool visible, Action doneCallback)
        {
            canvas.enabled = visible;
            doneCallback?.Invoke();
        }
    }
}