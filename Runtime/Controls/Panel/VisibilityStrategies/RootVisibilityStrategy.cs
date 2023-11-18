using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class RootVisibilityStrategy : VisibilityStrategyBase
    {
        [SerializeField] private RectTransform root;

        public override bool IsVisible
        {
            get { return root.gameObject.activeSelf; }
        }

        public override void SetVisible(bool visible, Action callback)
        {
            root.gameObject.SetActive(visible);
            callback?.Invoke();
        }
    }
}