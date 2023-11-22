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
        
        private readonly List<GameObject> items = new List<GameObject>();

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
            get { return items; }
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
            GameObject item = UIGameObjectFactory.Create(itemPrefab, root.transform);
            
            items.Insert(index, item);

            item.transform.SetSiblingIndex(index);
            item.SetActive(true);
                    
            return new ItemReport(item, true);
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

            items.RemoveAt(index);
                
            UIGameObjectFactory.Destroy(item);
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
            public Transform itemRoot;
            public GameObject itemPrefab;
        }
    }
}