using System;

namespace Rehawk.UIFramework
{
    public static class BindingOnChangedExtensions
    {
        public static Binding OnChanged(this Binding binding, Action callback)
        {
            binding.Changed += _ =>
            {
                callback.Invoke();
            };
            
            return binding;
        }
        
        public static Binding OnChanged(this Binding binding, ChangeOrigin origin, Action callback)
        {
            binding.Changed += changeOrigin =>
            {
                if (origin == changeOrigin)
                    callback.Invoke();
            };
            
            return binding;
        }
    }
}