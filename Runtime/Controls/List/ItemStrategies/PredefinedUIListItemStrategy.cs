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

        public IReadOnlyList<GameObject> Items
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

        public GameObject GetItem(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                return items[index];
            }

            return null;
        }
        
        public ItemReport SetItem(int index, object data)
        {
            GameObject item = GetItem(index);
            
            item.transform.SetSiblingIndex(index);
            item.SetActive(true);

            return new ItemReport(item, false);
        }

        public ItemReport AddItem(int index, object data)
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

        public void RemoveItem(int index)
        {
            items[index].SetActive(KeepEmptyActive);
            emptyItemsQueue.Enqueue(items[index]);
            emptyItems.Add(items[index]);
        }

        public void Clear()
        {
            for (int i = 0; i < items.Count; i++)
            {
                RemoveItem(i);
            }
        }

        [Serializable]
        public class Dependencies
        {
            public GameObject[] items;
        }
    }
}