using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public delegate void UIListItemCallbackDelegate(int index, GameObject item, object data);

    public abstract class UIListBase : UIControlBase
    {
        public abstract IEnumerable Items { get; set; }
        public abstract int Count { get; set; }
        public abstract IReadOnlyList<GameObject> ItemObjects { get; }
        public abstract void SetItemStrategy(IUIListItemStrategy itemStrategy);
        public abstract void SetItemReceiverType<T>() where T : IUIListItemReceiver;
        public abstract void SetItemCallback(UIListItemCallback type, UIListItemCallbackDelegate callback);
    }
}