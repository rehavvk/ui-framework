using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIImageGroup : UIImageBase
    {
        [SerializeField] private UIImageBase[] targets;

        private bool isVisible;
        private bool isEnabled;
        private Material material;
        private Color color;
        private Sprite sprite;
        private float fillAmount;
        
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

        public override Material Material
        {
            get { return material; }
            set 
            { 
                material = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Material = material;
                }
            }
        }

        public override Color Color
        {
            get { return color; }
            set 
            { 
                color = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Color = color;
                }
            }
        }

        public override Sprite Sprite
        {
            get { return sprite; }
            set 
            { 
                sprite = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Sprite = sprite;
                }
            }
        }

        public override float FillAmount
        {
            get { return fillAmount; }
            set 
            { 
                fillAmount = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].FillAmount = fillAmount;
                }
            }
        }
    }
}