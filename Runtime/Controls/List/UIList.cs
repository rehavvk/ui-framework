using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        public override IEnumerable Items
        {
            get { return items; }
            set
            {
                items = value;
                
                int newCount = 0;

                if (items != null)
                {
                    foreach (object data in items)
                    {
                        newDatasets.Add(data);
                        newCount += 1;
                    }
                }

                count = newCount;
                
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
            
            itemStrategy.RemoveAllItemObjects();
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

        private void RefreshItems()
        {
            for (int i = 0; i < datasets.Count; i++)
            {
                object oldData = null;
                if (i < datasets.Count)
                {
                    oldData = datasets[i];
                }

                GameObject item = itemStrategy.GetItemObject(i);
                
                if (i < count)
                {
                    object data = null;
                    if (i < newDatasets.Count)
                    {
                        data = newDatasets[i];
                    }

                    if (oldData != data)
                    {
                        InvokeCallback(UIListItemCallback.Deactivated, i, item, oldData);
                        
                        ItemReport report = itemStrategy.SetItemObject(i, data);
                        GameObject setItem = report.Object;
                        
                        InformListItemReceiver(setItem, i, data);
                        
                        if (report.IsNew)
                        {
                            InvokeCallback(UIListItemCallback.Initialized, i, setItem, data);
                        }
                    
                        InvokeCallback(UIListItemCallback.Activated, i, setItem, data);
                    }
                }
                else
                {
                    InvokeCallback(UIListItemCallback.Deactivated, i, item, oldData);
                    itemStrategy.DeactivateItemObject(item);
                }
            }
            
            if (itemStrategy.ItemObjects.Count < count)
            {
                for (int i = itemStrategy.ItemObjects.Count; i < count; i++)
                {
                    object data = null;
                    if (i < newDatasets.Count)
                    {
                        data = newDatasets[i];
                    }
                    
                    ItemReport report = itemStrategy.AddItemObject(i, data);
                    InformListItemReceiver(report.Object, i, data);

                    if (report.IsNew)
                    {
                        InvokeCallback(UIListItemCallback.Initialized, i, report.Object, data);
                    }
                    
                    InvokeCallback(UIListItemCallback.Activated, i, report.Object, data);
                }
            }

            itemStrategy.RemoveInactiveItemObjects();

            datasets.Clear();
            datasets.AddRange(newDatasets);
            newDatasets.Clear();
            
            OnPropertyChanged();
            OnPropertyChanged(nameof(Count));
        }

        private void InformListItemReceiver(GameObject item, int index, object data)
        {
            Type itemReceiverType = this.itemReceiverType;
                        
            if (itemReceiverType == null)
            {
                itemReceiverType = typeof(IUIListItemReceiver);
            }
                        
            if (item.TryGetComponent(itemReceiverType, out Component itemReceiverComponent) && itemReceiverComponent is IUIListItemReceiver itemReceiver)
            {
                itemReceiver.SetListItem(new ListItem(index, data));
            }
            else if (this.itemReceiverType != null)
            {
                Debug.LogError($"No fitting item receiver has been found. [requestedItemReceiver={this.itemReceiverType}]", item);
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