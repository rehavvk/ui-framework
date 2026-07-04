#if UNITY_VECTOR_GRAPHICS
using System;
using UnityEngine;
using Unity.VectorGraphics;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Represents a user interface image component.
    /// This class extends the functionality of <see cref="UIImageBase"/> and wrappers the <see cref="SVGImage"/> component.
    /// </summary>
    public class UISVGImage : UIImageBase
    {
        [SerializeField] private SVGImage target;

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

        public override Sprite Sprite
        {
            get => target.sprite;
            set
            {
                if (target.sprite == value)
                    return;
                
                target.sprite = value;
                OnPropertyChanged();
            }
        }

        public override Sprite OverrideSprite
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
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

        public override float FillAmount
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
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
                target = GetComponentInChildren<SVGImage>();
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            if (target == null)
            {
                target = GetComponentInChildren<SVGImage>();
            }
        }
#endif
    }
}
#endif