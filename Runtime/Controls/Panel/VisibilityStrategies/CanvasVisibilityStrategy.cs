using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Implements a visibility strategy for controlling the visibility of a UI panel
    /// using a <see cref="Canvas"/> component. This class enables or disables the Canvas
    /// to show or hide its associated UI panel.
    /// </summary>
    [Serializable]
    public class CanvasVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private Canvas canvas;

        public override bool IsVisible => canvas.enabled;

        public override void SetVisible(UIPanelBase panel, bool visible, bool instant, Action doneCallback)
        {
            canvas.enabled = visible;
            doneCallback?.Invoke();
        }
    }
}