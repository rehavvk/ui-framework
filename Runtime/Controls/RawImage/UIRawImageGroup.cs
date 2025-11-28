using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIRawImageGroup : UIRawImageBase
    {
        [TextArea(1, 10)]
        [SerializeField] private string documentation;
        [SerializeField] private UIRawImageBase[] targets;

        private bool isVisible;
        private bool isEnabled;
        private Material material;
        private Color color;
        private Texture texture;
        
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
            get { return Color.a; }
            set
            {
                var previousColor = Color;
                previousColor.a = value;
                Color = previousColor;
                
                OnPropertyChanged();
            }
        }

        public override Texture Texture
        {
            get { return texture; }
            set 
            { 
                texture = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Texture = texture;
                }
                
                OnPropertyChanged();
            }
        }
    }
}