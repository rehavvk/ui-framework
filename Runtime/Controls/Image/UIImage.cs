using UnityEngine;
using UnityEngine.UI;

namespace Rehawk.UIFramework
{
    public class UIImage : UIImageBase
    {
        [SerializeField] private Image target;

        public override bool IsVisible
        {
            get { return target.gameObject.activeSelf; }
            set 
            {
                target.gameObject.SetActive(value);
                OnPropertyChanged();
            }
        }

        public override bool Enabled
        {
            get { return target.enabled; }
            set
            {
                if (target.enabled != value)
                {
                    target.enabled = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public override Sprite Sprite
        {
            get { return target.sprite; }
            set
            {
                if (target.sprite != value)
                {
                    target.sprite = value;
                    OnPropertyChanged();
                }
            }
        }

        public override Sprite OverrideSprite
        {
            get { return target.overrideSprite; }
            set
            {
                if (target.overrideSprite != value)
                {
                    target.overrideSprite = value;
                    OnPropertyChanged();
                }
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

        public override float Alpha
        {
            get { return Color.a; }
            set
            {
                if (Color.a != value)
                {
                    var previousColor = Color;
                    previousColor.a = value;
                    Color = previousColor;
                    OnPropertyChanged();
                }
            }
        }

        public override float FillAmount
        {
            get { return target.fillAmount; }
            set
            {
                if (target.fillAmount != value)
                {
                    target.fillAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public void SetNativeSize()
        {
            target.SetNativeSize();    
        }
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            
            if (target == null)
            {
                target = GetComponentInChildren<Image>();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target == null)
            {
                target = GetComponentInChildren<Image>();
            }
        }
#endif
    }
}