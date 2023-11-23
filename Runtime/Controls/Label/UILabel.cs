using TMPro;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UILabel : UILabelBase
    {
        [SerializeField] private TextMeshProUGUI target;
        
        private IUILabelTextStrategy strategy;

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

        public override Material Material
        {
            get { return target.material; }
            set 
            {
                if (target.material != value)
                {
                    target.material = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string Text
        {
            get { return strategy.GetText(this); }
            set
            {
                if (strategy.SetText(this, value))
                {
                    OnPropertyChanged();
                }
            }
        }

        public override Color Color
        {
            get { return target.color; }
            set
            {
                if (target.color != value)
                {
                    target.color = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (strategy == null)
            {
                SetStrategy(new DefaultTextStrategy());
            }
        }

        public override void SetStrategy(IUILabelTextStrategy strategy)
        {
            if (this.strategy != null)
            {
                this.strategy.TextChanged -= OnTextChanged;
            }
            
            this.strategy = strategy;
            
            if (this.strategy != null)
            {
                this.strategy.TextChanged += OnTextChanged;
            }
        }

        private void OnTextChanged(string text)
        {
            target.text = text;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            
            if (target == null)
            {
                target = GetComponentInChildren<TextMeshProUGUI>();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target == null)
            {
                target = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
#endif
    }
}