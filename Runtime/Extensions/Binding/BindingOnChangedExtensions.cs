using System;

namespace Rehawk.UIFramework
{
    public static class BindingOnChangedExtensions
    {
        public static Binding OnChanged(this Binding binding, Action callback)
        {
            binding.Evaluated += _ =>
            {
                callback.Invoke();
            };
            
            return binding;
        }
        
        public static Binding OnChanged(this Binding binding, Action<EvaluationDirection> callback)
        {
            binding.Evaluated += callback;
            
            return binding;
        }
    }
}