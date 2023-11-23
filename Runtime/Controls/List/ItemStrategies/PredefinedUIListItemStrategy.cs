using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// List strategy that allows the use of any number of predefined items. 
    /// Ignores any further data if the number of predefined items is exceeded.
    /// </summary>
    public class PredefinedUIListItemStrategy : IUIListItemStrategy
    {
        private readonly List<GameObject> itemObjects = new List<GameObject>();
        private readonly List<GameObject> newItemObjects = new List<GameObject>();
        private readonly List<GameObject> inactiveItemObjects = new List<GameObject>();
        private readonly List<GameObject> emptyItemObjects = new List<GameObject>();
        private readonly Queue<GameObject> emptyItemObjectsQueue = new Queue<GameObject>();

        private bool keepEmptyActive;
        
        public PredefinedUIListItemStrategy(GameObject[] itemObjects)
        {
            this.itemObjects.AddRange(itemObjects);
            newItemObjects.AddRange(itemObjects);
            emptyItemObjects.AddRange(itemObjects);
            
            for (int i = 0; i < this.itemObjects.Count; i++)
            {
                GameObject item = this.itemObjects[i];
                
                item.SetActive(KeepEmptyActive);
                emptyItemObjectsQueue.Enqueue(item);
            }
        }
        
        public PredefinedUIListItemStrategy(Dependencies dependencies) : this(dependencies.itemObjects) { }

        public IReadOnlyList<GameObject> ItemObjects
        {
            get { return itemObjects; }
        }

        public bool KeepEmptyActive
        {
            get { return keepEmptyActive; }
            set
            {
                keepEmptyActive = value;
                
                for (int i = 0; i < emptyItemObjects.Count; i++)
                {
                    GameObject item = emptyItemObjects[i];
                
                    item.SetActive(KeepEmptyActive);
                }
            }
        }

        public GameObject GetItemObject(int index)
        {
            if (index >= 0 && index < itemObjects.Count)
            {
                return itemObjects[index];
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
            
            if (emptyItemObjectsQueue.Count > 0)
            {
                GameObject item = emptyItemObjectsQueue.Dequeue();
                emptyItemObjects.Remove(item);

                bool isNew = newItemObjects.Remove(item);
                
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

        public void DeactivateItemObject(GameObject itemObject)
        {
            inactiveItemObjects.Add(itemObject);
        }
        
        public void RemoveInactiveItemObjects()
        {
            for (int i = inactiveItemObjects.Count - 1; i >= 0; i--)
            {
                RemoveItemObject(inactiveItemObjects[i]);
            }
        }
        
        public void RemoveAllItemObjects()
        {
            for (int i = itemObjects.Count - 1; i >= 0; i--)
            {
                RemoveItemObject(itemObjects[i]);
            }
        }
        
        private void RemoveItemObject(GameObject itemObject)
        {
            if (itemObject == null)
            {
                return;
            }
            
            int index = itemObjects.IndexOf(itemObject);

            if (index < 0)
            {
                return;
            }

            itemObject.SetActive(KeepEmptyActive);
            emptyItemObjectsQueue.Enqueue(itemObject);
            emptyItemObjects.Add(itemObject);
        }

        [Serializable]
        public class Dependencies
        {
            [FormerlySerializedAs("items")]
            public GameObject[] itemObjects;
        }
    }
}