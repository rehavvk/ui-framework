using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIElementGroup : UIElementBase
    {
        [SerializeField] private UIElementBase[] targets;

        private bool isVisible;
            
        public override bool IsVisible
        {
            get { return isVisible; }
            set 
            { 
                isVisible = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].IsVisible = isVisible;
                }
            }
        }
    }
}