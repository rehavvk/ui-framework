using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIButtonGroup : UIButtonBase
    {
        [TextArea(1, 10)]
        [SerializeField] private string documentation;
        [SerializeField] private UIButtonBase[] targets;
        
        private bool isVisible;
        private bool isEnabled;
        private bool isInteractable;
        private ICommand clickCommand;
        private ICommand hoverBeginCommand;
        private ICommand hoverEndCommand;

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
            get { return enabled; }
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

        public override ICommand ClickCommand
        {
            get { return clickCommand; }
            set 
            { 
                clickCommand = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].ClickCommand = clickCommand;
                }
            }
        }

        public override ICommand HoverBeginCommand
        {
            get { return hoverBeginCommand; }
            set 
            { 
                hoverBeginCommand = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].HoverBeginCommand = hoverBeginCommand;
                }
            }
        }

        public override ICommand HoverEndCommand
        {
            get { return hoverEndCommand; }
            set 
            { 
                hoverEndCommand = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].HoverEndCommand = hoverEndCommand;
                }
            }
        }
    }
}