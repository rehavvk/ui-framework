using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Defines the root visibility strategy for UI panels. This strategy controls
    /// the visibility of a panel by toggling the active state of the associated root
    /// GameObject.
    /// </summary>
    [Serializable]
    public class RootVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private RectTransform root;

        public override bool IsVisible => root.gameObject.activeSelf;

        public override void SetVisible(UIPanelBase panel, bool visible, bool instant, Action doneCallback)
        {
            root.gameObject.SetActive(visible);
            doneCallback?.Invoke();
        }
    }
}