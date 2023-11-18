using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIList : UIListBase
    {
        private IUIListItemStrategy itemStrategy;
        private Type itemReceiverType;

        private readonly List<object> datasets = new List<object>();
        private readonly List<object> newDatasets = new List<object>();
        private int itemCount;
        
        private UIListItemCallbackDelegate onInitialized;
        private UIListItemCallbackDelegate onActivated;
        private UIListItemCallbackDelegate onDeactivated;

        public override IReadOnlyList<GameObject> Items
        {
            get { return itemStrategy.Items; }    
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            itemStrategy.Clear();
        }

        protected override void AfterContextChanged()
        {
            base.AfterContextChanged();

            RefreshItems();
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
            itemCount = 0;
            
            switch (RawContext)
            {
                case IEnumerable enumerableContext:
                {
                    foreach (object data in enumerableContext)
                    {
                        newDatasets.Add(data);
                        itemCount += 1;
                    }

                    break;
                }
                case int intContext:
                    itemCount = intContext;
                    break;
            }
            
            for (int i = 0; i < datasets.Count; i++)
            {
                object oldData = null;
                if (i < datasets.Count)
                {
                    oldData = datasets[i];
                }

                GameObject item = itemStrategy.GetItem(i);
                
                if (i < itemCount)
                {
                    object data = null;
                    if (i < newDatasets.Count)
                    {
                        data = newDatasets[i];
                    }

                    if (oldData != data)
                    {
                        InvokeCallback(UIListItemCallback.Deactivated, i, item, oldData);
                        
                        ItemReport report = itemStrategy.SetItem(i, data);
                        InformListItemReceiver(item, i, data);
                        
                        if (report.IsNew)
                        {
                            InvokeCallback(UIListItemCallback.Initialized, i, report.Object, data);
                        }
                    
                        InvokeCallback(UIListItemCallback.Activated, i, report.Object, data);
                    }
                }
                else
                {
                    InvokeCallback(UIListItemCallback.Deactivated, i, item, oldData);
                    itemStrategy.RemoveItem(i);
                }
            }
                
            if (itemStrategy.Items.Count < itemCount)
            {
                for (int i = itemStrategy.Items.Count; i < itemCount; i++)
                {
                    object data = null;
                    if (i < newDatasets.Count)
                    {
                        data = newDatasets[i];
                    }
                    
                    ItemReport report = itemStrategy.AddItem(i, data);
                    InformListItemReceiver(report.Object, i, data);

                    if (report.IsNew)
                    {
                        InvokeCallback(UIListItemCallback.Initialized, i, report.Object, data);
                    }
                    
                    InvokeCallback(UIListItemCallback.Activated, i, report.Object, data);
                }
            }
            
            datasets.Clear();
            datasets.AddRange(newDatasets);
            newDatasets.Clear();
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