using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public delegate void UIListItemCallbackDelegate(int index, GameObject item, object data);

    // TODO: Streamlining would be nice. UIList is the only element which is specially binded via context.

    public abstract class UIListBase : UIContextControlBase
    {
        public abstract IReadOnlyList<GameObject> Items { get; }
        public abstract void SetItemStrategy(IUIListItemStrategy itemStrategy);
        public abstract void SetItemReceiverType<T>() where T : IUIListItemReceiver;
        public abstract void SetItemCallback(UIListItemCallback type, UIListItemCallbackDelegate callback);
    }
}