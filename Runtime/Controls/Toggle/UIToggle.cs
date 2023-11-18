using UnityEngine;
using UnityEngine.UI;

namespace Rehawk.UIFramework
{
    public class UIToggle : UIToggleBase
    {
        [SerializeField] private Toggle target;
        
        private ICommand changedCommand;

        public override bool IsVisible
        {
            get { return gameObject.activeSelf; }
            set 
            {
                gameObject.SetActive(value);
                OnPropertyChanged();
            }
        }

        public override bool Enabled
        {
            get { return target.enabled; }
            set 
            {
                target.enabled = value;
                OnPropertyChanged();
            }
        }

        public override bool IsInteractable
        {
            get { return target.interactable; }
            set
            {
                target.interactable = value;
                OnPropertyChanged();
            }
        }

        public override bool Value
        {
            get { return target.isOn; }
            set { target.isOn = value; }
        }

        public override object BoxedValue
        {
            get { return target.isOn; }
            set { target.isOn = value != null; }
        }

        public override ICommand ChangedCommand
        {
            get { return changedCommand; }
            set { SetField(ref changedCommand, value); }
        }

        protected override void Awake()
        {
            base.Awake();

            if (target)
            {
                target.onValueChanged.AddListener(OnValueChanged);
            }
            
            IsInteractable = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (target)
            {
                target.onValueChanged.RemoveListener(OnValueChanged);
            }
        }

        private void OnValueChanged(bool isOn)
        {
            OnPropertyChanged(nameof(Value));
            
            if (ChangedCommand != null && ChangedCommand.CanExecute(null))
            {
                ChangedCommand?.Execute(isOn);
            }
        }
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            
            if (target == null)
            {
                target = GetComponentInChildren<Toggle>();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target == null)
            {
                target = GetComponentInChildren<Toggle>();
            }
        }
#endif
    }
}