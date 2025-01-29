using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIGraphicGroup : UIGraphicBase
    {
        [SerializeField] private UIGraphicBase[] targets;

        private bool isVisible;
        private bool isEnabled;
        private Material material;
        private Color color;
        private float alpha;
        
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
            get { return isEnabled; }
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
                
                OnPropertyChanged();
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
                
                OnPropertyChanged();
            }
        }

        public override float Alpha
        {
            get { return alpha; }
            set 
            { 
                alpha = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Alpha = alpha;
                }
                
                OnPropertyChanged();
            }
        }
    }
}