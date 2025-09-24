using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rehawk.UIFramework
{
    public interface IUIControl : INotifyPropertyChanged
    {
        Binding Bind<T>(Expression<Func<T>> memberExpression, BindingDirection direction = BindingDirection.OneWay);
        Binding BindProperty(Func<object> getContext, string propertyName, BindingDirection direction = BindingDirection.OneWay);
        Binding BindContext(Func<UIContextControlBase> getControlCallback, BindingDirection direction = BindingDirection.OneWay);
        Binding BindCallback<T>(Action<T> setCallback);
        Binding BindCallback<T>(Func<T> getCallback, Action<T> setCallback);
        
        void SetDirty();
    }
    
    public abstract class UIControlBase : UIBehaviour, IUIControl, INotifyPropertyChanged
    {
        private UIPanel parentPanel;
        
        private readonly List<Binding> bindings = new List<Binding>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected override void Awake()
        {
            base.Awake();
            
            parentPanel = GetComponentInParent<UIPanel>();

            if (parentPanel)
            {
                parentPanel.BecameVisible += OnPanelBecameVisible;
                parentPanel.BecameInvisible += OnPanelBecameInvisible;
            }
        }

        protected override void Start()
        {
            base.Start();
            
            SetupBindingsInternal();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (parentPanel)
            {
                parentPanel.BecameVisible -= OnPanelBecameVisible;
                parentPanel.BecameInvisible -= OnPanelBecameInvisible;
            }

            ReleaseBindings();
        }
        
        protected override void OnTransformParentChanged()
        {
            base.OnTransformParentChanged();

            if (parentPanel)
            {
                parentPanel.BecameVisible -= OnPanelBecameVisible;
                parentPanel.BecameInvisible -= OnPanelBecameInvisible;
            }
            
            parentPanel = GetComponentInParent<UIPanel>();
            
            if (parentPanel)
            {
                parentPanel.BecameVisible += OnPanelBecameVisible;
                parentPanel.BecameInvisible += OnPanelBecameInvisible;
            }
        }
        
        private void SetupBindingsInternal()
        {
            SetupBindings();
            SetDirty();
        }
        
        protected virtual void SetupBindings() { }
        
        public Binding Bind<T>(Expression<Func<T>> memberExpression, BindingDirection direction = BindingDirection.OneWay)
        {
            var binding = Binding.BindMember(this, () => this, memberExpression, direction);
            
            bindings.Add(binding);
            
            return binding;
        }

        public Binding BindProperty(Func<object> getContext, string propertyName, BindingDirection direction = BindingDirection.OneWay)
        {
            var binding = Binding.BindProperty(this, getContext, propertyName, direction);
            
            bindings.Add(binding);
            
            return binding;
        }

        public Binding BindContext(Func<UIContextControlBase> getControlCallback, BindingDirection direction = BindingDirection.OneWay)
        {
            var binding = Binding.BindContext(this, getControlCallback, direction);
            
            bindings.Add(binding);
            
            return binding;
        }
        
        public Binding BindCallback<T>(Action<T> setCallback)
        {
            var binding = Binding.BindCallback<T>(this, () => default, setCallback);
            
            bindings.Add(binding);
            
            return binding;
        }
        
        public Binding BindCallback<T>(Func<T> getCallback, Action<T> setCallback)
        {
            var binding = Binding.BindCallback<T>(this, getCallback, setCallback);
            
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

        protected virtual void OnBecameVisible() {}
        protected virtual void OnBecameInvisible() {}
        
        private void ReleaseBindings()
        {
            for (int i = 0; i < bindings.Count; i++)
            {
                bindings[i].Release();
            }
        }

        private void OnPanelBecameVisible(UIPanel panel)
        {
            OnBecameVisible();
        }

        private void OnPanelBecameInvisible(UIPanel panel)
        {
            OnBecameInvisible();
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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