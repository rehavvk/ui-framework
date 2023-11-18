using UnityEngine;

namespace Rehawk.UIFramework
{
    public struct ItemReport
    {
        public GameObject Object { get; }
        public bool IsNew { get; }
        
        public ItemReport(GameObject obj, bool isNew)
        {
            Object = obj;
            IsNew = isNew;
        }
    }
}