using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// List strategy that creates new items for each data based on a single prefab.
    /// </summary>
    public class PrefabUIListItemStrategy : IUIListItemStrategy
    {
        private readonly Transform root;
        private readonly GameObject itemPrefab;
        
        private readonly List<GameObject> itemObjects = new List<GameObject>();
        private readonly List<GameObject> inactiveItemObjects = new List<GameObject>();

        public PrefabUIListItemStrategy(Transform root, GameObject itemPrefab)
        {
            this.root = root;
            this.itemPrefab = itemPrefab;

            if (!string.IsNullOrEmpty(itemPrefab.scene.name))
            {
                // Prefab is a scene object. Disable it.
                this.itemPrefab.SetActive(false);
            }
        }
        
        public PrefabUIListItemStrategy(Dependencies dependencies) : this(dependencies.itemRoot, dependencies.itemPrefab) { }
        
        public IReadOnlyList<GameObject> ItemObjects
        {
            get { return itemObjects; }
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
            GameObject item = UIGameObjectFactory.Create(itemPrefab, root.transform);
            
            itemObjects.Insert(index, item);

            item.transform.SetSiblingIndex(index);
            item.SetActive(true);
                    
            return new ItemReport(item, true);
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

            itemObjects.Remove(itemObject);
            inactiveItemObjects.Remove(itemObject);
                
            UIGameObjectFactory.Destroy(itemObject);
        }

        [Serializable]
        public class Dependencies
        {
            public Transform itemRoot;
            public GameObject itemPrefab;
        }
    }
}