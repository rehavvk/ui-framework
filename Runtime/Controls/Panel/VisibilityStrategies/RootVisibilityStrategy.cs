using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class RootVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private RectTransform root;

        public override bool IsVisible => root.gameObject.activeSelf;

        public override void SetVisible(UIPanelBase panel, bool visible, Action doneCallback)
        {
            root.gameObject.SetActive(visible);
            doneCallback?.Invoke();
        }
    }
}