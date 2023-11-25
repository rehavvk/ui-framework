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
        private readonly List<GameObject> activeItemObjects = new List<GameObject>();
        private readonly List<GameObject> freshItemObjects = new List<GameObject>();
        private readonly List<GameObject> inactiveItemObjects = new List<GameObject>();
        private readonly Queue<GameObject> inactiveItemObjectsQueue = new Queue<GameObject>();

        private bool keepEmptyActive;
        
        public PredefinedUIListItemStrategy(GameObject[] itemObjects)
        {
            freshItemObjects.AddRange(itemObjects);
            inactiveItemObjects.AddRange(itemObjects);
            
            for (int i = 0; i < itemObjects.Length; i++)
            {
                GameObject item = itemObjects[i];
                
                item.SetActive(KeepEmptyActive);
                inactiveItemObjectsQueue.Enqueue(item);
            }
        }
        
        public PredefinedUIListItemStrategy(Dependencies dependencies) : this(dependencies.itemObjects) { }

        public IReadOnlyList<GameObject> ItemObjects
        {
            get { return activeItemObjects; }
        }

        public bool KeepEmptyActive
        {
            get { return keepEmptyActive; }
            set
            {
                keepEmptyActive = value;
                
                for (int i = 0; i < inactiveItemObjects.Count; i++)
                {
                    GameObject item = inactiveItemObjects[i];
                
                    item.SetActive(KeepEmptyActive);
                }
            }
        }

        public GameObject GetItemObject(int index)
        {
            if (index >= 0 && index < activeItemObjects.Count)
            {
                return activeItemObjects[index];
            }

            return null;
        }
        
        public ItemReport SetItemObject(int index, object data)
        {
            GameObject item = GetItemObject(index);
            
            item.SetActive(true);

            return new ItemReport(item, false);
        }

        public ItemReport AddItemObject(int index, object data)
        {
            ItemReport addReport;
            
            if (inactiveItemObjectsQueue.Count > 0)
            {
                GameObject item = inactiveItemObjectsQueue.Dequeue();
                inactiveItemObjects.Remove(item);

                bool isNew = freshItemObjects.Remove(item);
                
                item.SetActive(true);
                
                addReport = new ItemReport(item, isNew);
            }
            else
            {
                addReport = new ItemReport(null, false);
                Debug.LogError($"<b>{nameof(PredefinedUIListItemStrategy)}:</b> Amount of predefined item objects exceeded.");
            }

            return addReport;
        }

        public void DeactivateItemObject(GameObject itemObject)
        {
            activeItemObjects.Remove(itemObject);
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
            for (int i = activeItemObjects.Count - 1; i >= 0; i--)
            {
                RemoveItemObject(activeItemObjects[i]);
            }
        }
        
        private void RemoveItemObject(GameObject itemObject)
        {
            if (itemObject == null || !inactiveItemObjects.Contains(itemObject))
            {
                return;
            }
            
            itemObject.SetActive(KeepEmptyActive);
            inactiveItemObjectsQueue.Enqueue(itemObject);
        }

        [Serializable]
        public class Dependencies
        {
            [FormerlySerializedAs("items")]
            public GameObject[] itemObjects;
        }
    }
}