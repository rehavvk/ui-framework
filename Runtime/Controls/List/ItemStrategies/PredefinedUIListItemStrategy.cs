using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// List strategy that allows the use of any number of predefined items. 
    /// Ignores any further data if the number of predefined items is exceeded.
    /// </summary>
    public class PredefinedUIListItemStrategy : IUIListItemStrategy
    {
        private readonly List<GameObject> items = new List<GameObject>();
        private readonly List<GameObject> newItems = new List<GameObject>();
        private readonly List<GameObject> emptyItems = new List<GameObject>();
        private readonly Queue<GameObject> emptyItemsQueue = new Queue<GameObject>();

        private bool keepEmptyActive;
        
        public PredefinedUIListItemStrategy(GameObject[] items)
        {
            this.items.AddRange(items);
            newItems.AddRange(items);
            emptyItems.AddRange(items);
            
            for (int i = 0; i < this.items.Count; i++)
            {
                GameObject item = this.items[i];
                
                item.SetActive(KeepEmptyActive);
                emptyItemsQueue.Enqueue(item);
            }
        }
        
        public PredefinedUIListItemStrategy(Dependencies dependencies) : this(dependencies.items) { }

        public IReadOnlyList<GameObject> ItemObjects
        {
            get { return items; }
        }

        public bool KeepEmptyActive
        {
            get { return keepEmptyActive; }
            set
            {
                keepEmptyActive = value;
                
                for (int i = 0; i < emptyItems.Count; i++)
                {
                    GameObject item = emptyItems[i];
                
                    item.SetActive(KeepEmptyActive);
                }
            }
        }

        public GameObject GetItemObject(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                return items[index];
            }

            return null;
        }
        
        public ItemReport SetItemObject(int index, object data)
        {
            GameObject item = GetItemObject(index);
            
            item.transform.SetSiblingIndex(index);
            item.SetActive(true);

            return new ItemReport(item, false);
        }

        public ItemReport AddItemObject(int index, object data)
        {
            ItemReport addReport;
            
            if (emptyItemsQueue.Count > 0)
            {
                GameObject item = emptyItemsQueue.Dequeue();
                emptyItems.Remove(item);

                bool isNew = newItems.Remove(item);
                
                item.SetActive(true);
                
                addReport = new ItemReport(item, isNew);
            }
            else
            {
                addReport = new ItemReport(null, false);
                Debug.LogError($"<b>{nameof(PredefinedUIListItemStrategy)}:</b> Amount of predefined items exceeded.");
            }

            return addReport;
        }

        public void RemoveItemObject(GameObject item)
        {
            if (item == null)
            {
                return;
            }
            
            int index = items.IndexOf(item);

            if (index < 0)
            {
                return;
            }

            item.SetActive(KeepEmptyActive);
            emptyItemsQueue.Enqueue(item);
            emptyItems.Add(item);
        }

        public void Clear()
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                RemoveItemObject(GetItemObject(i));
            }
        }

        [Serializable]
        public class Dependencies
        {
            public GameObject[] items;
        }
    }
}