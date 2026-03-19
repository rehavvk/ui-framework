using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public abstract class UIVirtualControlBase : IUIControl
    {
        private readonly List<Binding> bindings = new();
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void Init()
        {
            SetupBindingsInternal();
        }

        public void Release()
        {
            ReleaseBindings();       
        }
        
        private void SetupBindingsInternal()
        {
            SetupBindings();
            SetDirty();
        }
        
        protected virtual void SetupBindings() { }
        
        public Binding Bind<T>(Expression<Func<T>> memberExpression, BindingDirection direction = BindingDirection.OneWay)
        {
            Binding binding = Binding.BindMember(this, () => this, memberExpression, direction);
            
            bindings.Add(binding);
            
            return binding;
        }

        public Binding BindProperty(Func<object> getContext, string propertyName, BindingDirection direction = BindingDirection.OneWay)
        {
            Binding binding = Binding.BindProperty(this, getContext, propertyName, direction);
            
            bindings.Add(binding);
            
            return binding;
        }

        public Binding BindContext(Func<UIContextControlBase> getControlCallback, BindingDirection direction = BindingDirection.OneWay)
        {
            Binding binding = Binding.BindContext(this, getControlCallback, direction);
            
            bindings.Add(binding);
            
            return binding;
        }
        
        public Binding BindEvent(Action<Action> subscribe, Action<Action> unsubscribe)
        {
            Binding binding = Binding.BindEvent(this, subscribe, unsubscribe);

            bindings.Add(binding);

            return binding;
        }

        public Binding BindEvent<T>(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe)
        {
            Binding binding = Binding.BindEvent<T>(this, subscribe, unsubscribe);

            bindings.Add(binding);

            return binding;
        }

        public Binding BindEvent<T1, T2>(Action<Action<T1, T2>> subscribe, Action<Action<T1, T2>> unsubscribe)
        {
            Binding binding = Binding.BindEvent<T1, T2>(this, subscribe, unsubscribe);

            bindings.Add(binding);

            return binding;
        }

        public Binding BindEvent<T1, T2, T3>(Action<Action<T1, T2, T3>> subscribe, Action<Action<T1, T2, T3>> unsubscribe)
        {
            Binding binding = Binding.BindEvent<T1, T2, T3>(this, subscribe, unsubscribe);

            bindings.Add(binding);

            return binding;
        }

        public Binding BindCallback(Action callback)
        {
            Binding binding = Binding.BindCallback(this, callback);

            bindings.Add(binding);

            return binding;
        }

        public Binding BindCallback<T>(Action<T> setCallback)
        {
            Binding binding = Binding.BindCallback<T>(this, () => default, setCallback);

            bindings.Add(binding);

            return binding;
        }
        
        public Binding BindCallback<T>(Func<T> getCallback, Action<T> setCallback)
        {
            Binding binding = Binding.BindCallback<T>(this, getCallback, setCallback);
            
            bindings.Add(binding);
            
            return binding;
        }
        
        [ContextMenu("Set Dirty")]
        public void SetDirty()
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                bindings[i].Evaluate();
                bindings[i].SourceToDestination();
            }
        }

        /// <summary>
        /// Marks bindings associated with the specified tags as dirty, causing them to be re-evaluated.
        /// </summary>
        /// <param name="tags">An array of tags used to identify which bindings should be marked as dirty.</param>
        protected void SetDirty(params string[] tags)
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                if (bindings[i].Tags.Any(tag => tags.Contains(tag)))
                {
                    bindings[i].Evaluate();
                    bindings[i].SourceToDestination();
                }
            }
        }

        private void ReleaseBindings()
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                bindings[i].Release();
            }
        }
        
        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// This method invokes the <see cref="PropertyChanged"/> event with the provided property name.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that changed. This parameter is optional and can be automatically
        /// populated with the caller member name if not provided.
        /// </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Updates the specified field with a new value if it differs from the current value. Raises the <see cref="PropertyChanged"/> event if the value changes.
        /// </summary>
        /// <typeparam name="T">The type of the field to be updated.</typeparam>
        /// <param name="field">A reference to the field being updated.</param>
        /// <param name="value">The new value to set to the field.</param>
        /// <param name="propertyName">The name of the property invoking this method, automatically provided if called from a property setter.</param>
        /// <returns>Returns true if the field value was updated, otherwise returns false.</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            
            return true;
        }
    }
}