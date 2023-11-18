using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Rehawk.UIFramework
{
    public static class BindingConnectionExtensions
    {
        public static Binding ReevaluateWhenChanged<T>(this Binding binding, Expression<Func<T>> memberExpression, BindingConnectionDirection direction = BindingConnectionDirection.SourceToDestination) 
        {
            binding.ConnectTo(memberExpression, direction);
            
            return binding;
        }

        public static Binding ReevaluateWhenChanged(this Binding binding, Func<INotifyPropertyChanged> getContextFunction, string propertyName, BindingConnectionDirection direction = BindingConnectionDirection.SourceToDestination) 
        {
            binding.ConnectTo(getContextFunction, propertyName, direction);
            
            return binding;
        }
    }
}