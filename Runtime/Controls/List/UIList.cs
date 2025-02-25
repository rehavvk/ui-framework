using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIList : UIListBase
    {
        private readonly List<object> datasets = new List<object>();
        private readonly List<object> newDatasets = new List<object>();
        
        private IUIListItemStrategy itemStrategy;
        private Type itemReceiverType;
        
        private IEnumerable items;
        private int count;
        
        private UIListItemCallbackDelegate onInitialized;
        private UIListItemCallbackDelegate onActivated;
        private UIListItemCallbackDelegate onDeactivated;

        private readonly Dictionary<object, GameObject> oldDataToItem = new Dictionary<object, GameObject>();

        public override IEnumerable Items
        {
            get { return items; }
            set
            {
                items = value;
                
                if (items != null)
                {
                    foreach (object data in items)
                    {
                        newDatasets.Add(data);
                    }
                }
                
                count = newDatasets.Count;
                
                RefreshItems();
            }
        }

        public override int Count
        {
            get { return count; }
            set
            {
                if (SetField(ref count, value))
                {
                    RefreshItems();
                }
            }
        }

        public override IReadOnlyList<GameObject> ItemObjects
        {
            get { return itemStrategy.ItemObjects; }    
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            itemStrategy?.RemoveAllItemObjects();
        }

        public override void SetItemStrategy(IUIListItemStrategy itemStrategy)
        {
            this.itemStrategy = itemStrategy;
        }

        public override void SetItemReceiverType<T>()
        {
            itemReceiverType = typeof(T);
        }

        public override void SetItemCallback(UIListItemCallback type, UIListItemCallbackDelegate callback)
        {
            switch (type)
            {
                case UIListItemCallback.Initialized:
                    if (onInitialized != null)
                    {
                        Debug.LogError($"The callback was already set before. [callback={type}]", gameObject);
                    }
                    onInitialized = callback;
                    break;
                case UIListItemCallback.Activated:
                    if (onActivated != null)
                    {
                        Debug.LogError($"The callback was already set before. [callback={type}]", gameObject);
                    }
                    onActivated = callback;
                    break;
                case UIListItemCallback.Deactivated:
                    if (onDeactivated != null)
                    {
                        Debug.LogError($"The callback was already set before. [callback={type}]", gameObject);
                    }
                    onDeactivated = callback;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
        private void InvokeCallback(UIListItemCallback type, int index, GameObject item, object data)
        {
            switch (type) 
            {
                case UIListItemCallback.Initialized:
                    onInitialized?.Invoke(index, item, data);
                    break;
                case UIListItemCallback.Activated:
                    onActivated?.Invoke(index, item, data);
                    break;
                case UIListItemCallback.Deactivated:
                    onDeactivated?.Invoke(index, item, data);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private readonly List<GameObject> deactivatedItemObjects = new List<GameObject>();
        
        private void RefreshItems()
        {
            if (datasets.Count > 0)
            {
                for (int i = 0; i < datasets.Count; i++)
                {
                    object oldData = datasets[i];

                    GameObject item = itemStrategy.GetItemObject(i);

                    if (newDatasets.Contains(oldData))
                    {
                        oldDataToItem[oldData] = item;
                    }
                    
                    InvokeCallback(UIListItemCallback.Deactivated, i, item, oldData);
                    itemStrategy.DeactivateItemObject(item);
                    
                    deactivatedItemObjects.Add(item);
                }
            }
            else
            {
                int itemCount = itemStrategy.ItemObjects.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    GameObject item = itemStrategy.GetItemObject(i);
                    
                    InvokeCallback(UIListItemCallback.Deactivated, i, item, null);
                    itemStrategy.DeactivateItemObject(item);
                    
                    deactivatedItemObjects.Add(item);
                }
            }

            for (int i = 0; i < Count; i++)
            {
                object newData = null;
                if (i < newDatasets.Count)
                {
                    newData = newDatasets[i];
                }
                
                if (newData != null && oldDataToItem.TryGetValue(newData, out GameObject item) && item != null)
                {
                    itemStrategy.SetItemObject(i, item, newData);
                }
                else if (i < itemStrategy.ItemObjects.Count)
                {
                    item = itemStrategy.GetItemObject(i);
                    itemStrategy.SetItemObject(i, newData);
                }
                else
                {
                    ItemReport report = itemStrategy.AddItemObject(i, newData);
                    item = report.Object;

                    if (report.IsNew)
                    {
                        InvokeCallback(UIListItemCallback.Initialized, i, item, newData);
                    }
                }

                deactivatedItemObjects.Remove(item);
                
                InvokeCallback(UIListItemCallback.Activated, i, item, newData);
                
                InformListItemReceiver(item, i, newData);
            }

            for (int i = 0; i < deactivatedItemObjects.Count; i++)
            {
                InformListItemReceiver(deactivatedItemObjects[i], -1, null);
            }
            
            itemStrategy.RemoveInactiveItemObjects();

            datasets.Clear();
            datasets.AddRange(newDatasets);
            newDatasets.Clear();
            oldDataToItem.Clear();
            deactivatedItemObjects.Clear();

            OnPropertyChanged();
            OnPropertyChanged(nameof(Count));
        }

        private void InformListItemReceiver(GameObject item, int index, object data)
        {
            IUIListItemReceiver itemReceiver;
            
            if (itemReceiverType != null)
            {
                Component itemReceiverComponent = item.GetComponent(itemReceiverType);
                itemReceiver = itemReceiverComponent as IUIListItemReceiver;
            }
            else
            {
                itemReceiver = item.GetComponent<IUIListItemReceiver>();
            }
            
            if (itemReceiver != null)
            {
                itemReceiver.SetListItem(new ListItem(index, data));

                if (itemReceiverType == null)
                {
                    Debug.LogWarning($"No item receiver has been setup. First component implementing IUIListItemReceiver was used. [itemReceiver={itemReceiver.GetType()}]", item);
                }
            }
            else if (itemReceiverType != null)
            {
                Debug.LogError($"No item receiver has been found. [requestedItemReceiver={itemReceiverType}]", item);
            }
        }
    }

    public enum UIListItemCallback
    {
        Initialized,
        Activated,
        Deactivated,
    }
}