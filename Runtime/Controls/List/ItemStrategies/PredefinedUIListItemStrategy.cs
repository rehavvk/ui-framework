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
        private readonly List<GameObject> itemObjects = new List<GameObject>();
        private readonly List<GameObject> freshItemObjects = new List<GameObject>();
        private readonly List<GameObject> inactiveItemObjects = new List<GameObject>();
        private readonly List<GameObject> activeItemObjects = new List<GameObject>();

        private bool keepEmptyActive;
        
        public PredefinedUIListItemStrategy(IReadOnlyList<GameObject> itemObjects, bool keepEmptyActive)
        {
            this.keepEmptyActive = keepEmptyActive;

            this.itemObjects.AddRange(itemObjects);
            freshItemObjects.AddRange(itemObjects);
            inactiveItemObjects.AddRange(itemObjects);
            
            for (int i = 0; i < itemObjects.Count; i++)
            {
                GameObject item = itemObjects[i];
                
                item.SetActive(KeepEmptyActive);
            }
        }
        
        public PredefinedUIListItemStrategy(Dependencies dependencies) : this(dependencies.itemObjects, dependencies.keepEmptyActive) { }

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
            if (index >= 0 && index < itemObjects.Count)
            {
                return itemObjects[index];
            }

            return null;
        }
        
        public ItemReport SetItemObject(int index, object data)
        {
            ItemReport addReport;

            if (index >= 0 && index < itemObjects.Count)
            {
                GameObject itemObject = GetItemObject(index);
                addReport = SetItemObject(index, itemObject, data);
            }
            else
            {
                addReport = new ItemReport(null, false);
                Debug.LogError($"<b>{nameof(PredefinedUIListItemStrategy)}:</b> Index is outside of predefined item object range. [index={index}]");
            }
            
            return addReport;
        }

        public ItemReport SetItemObject(int index, GameObject itemObject, object data)
        {
            inactiveItemObjects.Remove(itemObject);

            if (!activeItemObjects.Contains(itemObject))
            {
                activeItemObjects.Add(itemObject);
            }
            
            bool isNew = freshItemObjects.Remove(itemObject);

            itemObject.SetActive(true);
                
            return new ItemReport(itemObject, isNew);
        }

        public ItemReport AddItemObject(int index, object data)
        {
            return SetItemObject(index, data);
        }

        public void DeactivateItemObject(GameObject itemObject)
        {
            if (itemObject == null || !itemObjects.Contains(itemObject) || inactiveItemObjects.Contains(itemObject))
            {
                return;
            }

            itemObject.SetActive(KeepEmptyActive);
            activeItemObjects.Remove(itemObject);
            inactiveItemObjects.Add(itemObject);
        }
        
        public void RemoveInactiveItemObjects()
        {
            // TODO: Nothing todo, they are already inactive and will not get destroyed.
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
            DeactivateItemObject(itemObject);
        }

        [Serializable]
        public class Dependencies
        {
            public bool keepEmptyActive;
            public GameObject[] itemObjects;
        }
    }
}