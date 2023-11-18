using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rehawk.UIFramework
{
    public class UIButton : UIButtonBase, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private bool isInteractable = true;
        private bool isEnabled = true;

        private Button button;
        private ICommand clickCommand;
        private ICommand hoverBeginCommand;
        private ICommand hoverEndCommand;

        public override ICommand ClickCommand
        {
            get { return clickCommand; }
            set
            {
                if (clickCommand != null)
                {
                    clickCommand.CanExecuteChanged -= OnClickCommandCanExecuteChanged;
                }

                if (SetField(ref clickCommand, value))
                {
                    ReevaluateInteractableState();
                }
                
                if (clickCommand != null)
                {
                    clickCommand.CanExecuteChanged += OnClickCommandCanExecuteChanged;
                }
            }
        }
        
        public override ICommand HoverBeginCommand
        {
            get { return hoverBeginCommand; }
            set { SetField(ref hoverBeginCommand, value); }
        }
        
        public override ICommand HoverEndCommand
        {
            get { return hoverEndCommand; }
            set { SetField(ref hoverEndCommand, value); }
        }
        
        public override bool IsVisible
        {
            get
            {
                if (button)
                {
                    return button.gameObject.activeSelf;
                }

                return gameObject.activeSelf;
            }
            set
            {
                if (button)
                {
                    button.gameObject.SetActive(value);
                }
                else
                {
                    gameObject.SetActive(value);
                }
                
                OnPropertyChanged();
            }
        }

        public override bool Enabled
        {
            get 
            { 
                if (button)
                {
                    return button.enabled;
                }

                return isEnabled;
            }
            set 
            {
                if (button)
                {
                    button.enabled = value;
                }

                isEnabled = value;
                OnPropertyChanged();
                ReevaluateInteractableState();
            }
        }

        public override bool IsInteractable
        {
            get
            {
                if (button)
                {
                    return button.interactable;
                }

                return isInteractable;
            }
            set
            {
                if (button)
                {
                    button.interactable = value;
                }

                isInteractable = value;
                OnPropertyChanged();
            }
        }

        protected override void Awake()
        {
            base.Awake();

            button = GetComponentInChildren<Button>();
            
            IsInteractable = false;
        }

        private void ReevaluateInteractableState()
        {
            IsInteractable = Enabled && ClickCommand != null && ClickCommand.CanExecute(null);
        }
        
        private void OnClickCommandCanExecuteChanged()
        {
            ReevaluateInteractableState();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (IsInteractable && HoverBeginCommand != null && HoverBeginCommand.CanExecute(null))
            {
                HoverBeginCommand?.Execute(null);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (IsInteractable && HoverEndCommand != null && HoverEndCommand.CanExecute(null))
            {
                HoverEndCommand?.Execute(null);
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (IsInteractable && ClickCommand != null && ClickCommand.CanExecute(null))
            {
                ClickCommand?.Execute(null);
            }
        }
    }
}