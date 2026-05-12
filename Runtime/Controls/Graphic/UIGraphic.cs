using UnityEngine;
using UnityEngine.UI;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Represents a user interface graphic component.
    /// This class extends the functionality of <see cref="UIGraphicBase"/> and wrappers the <see cref="Graphic"/> component.
    /// </summary>
    public class UIGraphic : UIGraphicBase
    {
        [SerializeField] private Graphic target;

        public override bool Enabled
        {
            get => target.enabled;
            set
            {
                if (target.enabled == value) 
                    return;
                
                target.enabled = value;
                OnPropertyChanged();
            }
        }

        public override bool IsVisible
        {
            get => target.gameObject.activeSelf;
            set 
            {
                target.gameObject.SetActive(value);
                OnPropertyChanged();
            }
        }

        public override Material Material
        {
            get => target.material;
            set
            {
                if (target.material == value) 
                    return;
                
                target.material = value;
                OnPropertyChanged();
            }
        }

        public override Color Color
        {
            get => target.color;
            set
            {
                if (target.color == value)
                    return;
                
                target.color = value;
                OnPropertyChanged();
            }
        }

        public override float Alpha
        {
            get => Color.a;
            set
            {
                if (Mathf.Approximately(Color.a, value))
                    return;
                
                Color previousColor = Color;
                previousColor.a = value;
                Color = previousColor;
                OnPropertyChanged();
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            
            if (target == null)
            {
                target = GetComponentInChildren<Graphic>();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target == null)
            {
                target = GetComponentInChildren<Graphic>();
            }
        }
#endif
    }
}