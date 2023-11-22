using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIInteractableGroup : UIInteractableBase
    {
        [SerializeField] private UIInteractableBase[] targets;

        private bool isVisible;
        private bool isEnabled;
        private bool isInteractable;
        
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

        public override bool Enabled
        {
            get { return isEnabled; }
            set 
            { 
                isEnabled = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Enabled = isEnabled;
                }
            }
        }

        public override bool IsInteractable
        {
            get { return isInteractable; }
            set 
            { 
                isInteractable = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].IsInteractable = isInteractable;
                }
            }
        }
    }
}