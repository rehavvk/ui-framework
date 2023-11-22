using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public interface IUIListItemStrategy
    {
        IReadOnlyList<GameObject> ItemObjects { get; }
        GameObject GetItemObject(int index);
        ItemReport SetItemObject(int index, object data);
        ItemReport AddItemObject(int index, object data);
        void RemoveItemObject(GameObject itemObject);
        void Clear();
    }
}