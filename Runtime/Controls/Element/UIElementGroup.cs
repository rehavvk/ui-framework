using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public class UIElementGroup
    {
        [SerializeField] private UIElementBase[] elements;

        private bool isVisible;
            
        public bool IsVisible
        {
            get { return isVisible; }
            set 
            { 
                isVisible = value;
                for (int i = 0; i < elements.Length; i++)
                {
                    elements[i].IsVisible = isVisible;
                }
            }
        }
    }
}