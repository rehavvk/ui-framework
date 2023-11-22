﻿using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UILabelGroup : UILabelBase
    {
        [SerializeField] private UILabelBase[] targets;

        private bool isVisible;
        private bool isEnabled;
        private Material material;
        private Color color;
        private string text;
        
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

        public override string Text
        {
            get { return text; }
            set 
            { 
                text = value;
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].Text = text;
                }
            }
        }

        public override void SetStrategy(IUILabelTextStrategy strategy)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].SetStrategy(strategy);
            }
        }
    }
}