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
        
        public IReadOnlyList<GameObject> Items
        {
            get { return items; }
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
            GameObject item = UIGameObjectFactory.Create(itemPrefab, root.transform);
            
            items.Add(item);

            item.transform.SetSiblingIndex(index);
            item.SetActive(true);
                    
            return new ItemReport(item, true);
        }

        public void RemoveItem(int index)
        {
            UIGameObjectFactory.Destroy(items[index]);
            items.RemoveAt(index);
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
            public Transform itemRoot;
            public GameObject itemPrefab;
        }
    }
}