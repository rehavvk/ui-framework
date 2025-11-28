using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIElementGroup : UIElementBase
    {
        [TextArea(1, 10)]
        [SerializeField] private string documentation;
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