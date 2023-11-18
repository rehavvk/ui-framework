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
        
        private readonly List<GameObject> items = new List<GameObject>();
        private readonly List<GameObject> itemPrefabs = new List<GameObject>();
        
        public ContextualPrefabUIListItemStrategy(Transform root, GetPrefabFunctionDelegate getItemPrefab)
        {
            this.root = root;
            this.getItemPrefab = getItemPrefab;
        }
        
        public ContextualPrefabUIListItemStrategy(Dependencies dependencies, GetPrefabFunctionDelegate getItemPrefab) : this(dependencies.itemRoot, getItemPrefab) { }

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
            GameObject oldItem = GetItem(index);
            GameObject oldItemPrefab = itemPrefabs[index];
            
            GameObject itemPrefab = getItemPrefab.Invoke(index, data);

            if (itemPrefab != oldItemPrefab)
            {
                RemoveItem(index);
                return AddItem(index, data);
            }
            
            oldItem.transform.SetSiblingIndex(index);
            oldItem.SetActive(true);

            return new ItemReport(oldItem, false);
        }

        public ItemReport AddItem(int index, object data)
        {
            GameObject itemPrefab = getItemPrefab.Invoke(index, data);
            
            GameObject item = UIGameObjectFactory.Create(itemPrefab, root.transform);
            
            items.Add(item);
            itemPrefabs.Add(itemPrefab);

            item.transform.SetSiblingIndex(index);
            item.SetActive(true);

            return new ItemReport(item, true);
        }

        public void RemoveItem(int index)
        {
            UIGameObjectFactory.Destroy(items[index]);
            items.RemoveAt(index);
            itemPrefabs.RemoveAt(index);
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
        }
    }
}