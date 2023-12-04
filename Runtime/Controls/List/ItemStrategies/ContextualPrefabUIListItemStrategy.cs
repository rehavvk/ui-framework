using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public delegate GameObject GetPrefabFunctionDelegate(int index, object data);
    
    /// <summary>
    /// List strategy that allows to choose a different prefab dependent on index and data for each list item.
    /// </summary>
    public class ContextualPrefabUIListItemStrategy : IUIListItemStrategy
    {
        private readonly Transform root;
        private readonly GetPrefabFunctionDelegate getItemPrefab;
        
        private readonly List<GameObject> itemObjects = new List<GameObject>();
        private readonly List<GameObject> inactiveItemObjects = new List<GameObject>();
        private readonly List<GameObject> itemObjectPrefabs = new List<GameObject>();
        
        public ContextualPrefabUIListItemStrategy(Transform root, GetPrefabFunctionDelegate getItemPrefab)
        {
            this.root = root;
            this.getItemPrefab = getItemPrefab;
        }
        
        public ContextualPrefabUIListItemStrategy(Dependencies dependencies, GetPrefabFunctionDelegate getItemPrefab) : this(dependencies.itemRoot, getItemPrefab) { }

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
            GameObject oldItemObject = GetItemObject(index);
            GameObject oldItemPrefab = itemObjectPrefabs[index];
            
            GameObject itemObjectPrefab = getItemPrefab.Invoke(index, data);

            if (itemObjectPrefab != oldItemPrefab)
            {
                RemoveItemObject(oldItemObject);
                return AddItemObject(index, data);
            }
            
            oldItemObject.transform.SetSiblingIndex(index);
            oldItemObject.SetActive(true);

            return new ItemReport(oldItemObject, false);
        }

        public ItemReport AddItemObject(int index, object data)
        {
            GameObject itemObjectPrefab = getItemPrefab.Invoke(index, data);
            
            GameObject itemObject = UIGameObjectFactory.Create(itemObjectPrefab, root.transform);
            
            itemObjects.Insert(index, itemObject);
            itemObjectPrefabs.Insert(index, itemObjectPrefab);

            itemObject.transform.SetSiblingIndex(index);
            itemObject.SetActive(true);

            return new ItemReport(itemObject, true);
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
            itemObjectPrefabs.RemoveAt(index);
                
            UIGameObjectFactory.Destroy(itemObject);
        }

        [Serializable]
        public class Dependencies
        {
            public Transform itemRoot;
        }
    }
}