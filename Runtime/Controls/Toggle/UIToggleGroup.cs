using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIToggleGroup : UIToggleBase
    {
        [TextArea(1, 10)]
        [SerializeField] private string documentation;
        [SerializeField] private UIToggleBase[] targets;
        
        private bool isVisible;
        private bool isEnabled;
        private bool isInteractable;
        private object boxedValue;
        private ICommand changedCommand;
        private bool value;

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
                
                OnPropertyChanged();
            }
        }

        public override bool Enabled
        {
            get { return enabled; }
            set 
            { 
                isEnabled = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Enabled = isEnabled;
                }
                
                OnPropertyChanged();
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
                
                OnPropertyChanged();
            }
        }

        public override object BoxedValue
        {
            get { return boxedValue; }
            set 
            { 
                boxedValue = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].BoxedValue = boxedValue;
                }
                
                OnPropertyChanged();
            }
        }

        public override ICommand ChangedCommand
        {
            get { return changedCommand; }
            set 
            { 
                changedCommand = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].ChangedCommand = changedCommand;
                }
                
                OnPropertyChanged();
            }
        }
        
        public override bool Value
        {
            get { return value; }
            set 
            { 
                this.value = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Value = this.value;
                }
                
                OnPropertyChanged();
            }
        }
    }
}