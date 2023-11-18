using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class CanvasVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private Canvas canvas;

        public override bool IsVisible
        {
            get { return canvas.enabled; }
        }

        public override void SetVisible(bool visible, Action callback)
        {
            canvas.enabled = visible;
            callback?.Invoke();
        }
    }
}