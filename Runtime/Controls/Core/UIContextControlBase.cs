using System;
using System.ComponentModel;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public abstract class UIContextControlBase : UIControlBase
    {
        private object context;

        private bool isInitialized;
        
        public event Action ContextChanged;

        public bool HasContext
        {
            get { return context != default; }
        }

        public object RawContext
        {
            get { return context; }
        }

        protected override void Start()
        {
            base.Start();

            isInitialized = true;
        }

        public T GetContext<T>()
        {
            if (context is T castedContext)
            {
                return castedContext;
            }

            return default;
        }
        
        public bool TryGetContext<T>(out T context)
        {
            if (this.context is T castedValue)
            {
                context = castedValue;
                return true;
            }

            context = default;
            
            return false;
        }

        public void SetContext<T>(T context)
        {
            BeforeContextChanged();
            UnLinkINotifyPropertyChanged();
            
            this.context = context;

            LinkINotifyPropertyChanged();
            AfterContextChanged();

            EvaluateBindings();

            if (isInitialized)
            {
                SetDirty();
            }
            
            ContextChanged?.Invoke();
        }

        public void ClearContext()
        {
            SetContext<object>(null);
        }

        private void LinkINotifyPropertyChanged()
        {
            if (context is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += OnContextPropertyChanged;
            }
        }

        private void UnLinkINotifyPropertyChanged()
        {
            if (context is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= OnContextPropertyChanged;
            }
        }

        /// <summary>
        /// Is called before the context is switched to another instance or cleared.
        /// </summary>
        protected virtual void BeforeContextChanged() {}
        
        /// <summary>
        /// Is called after the context is switched to another instance or cleared.
        /// </summary>
        protected virtual void AfterContextChanged() {}
        
        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetDirty();
        }
    }
    
    public abstract class UIContextControlBase<T> : UIContextControlBase
    {
        private T castedContext;

        public T Context
        {
            get { return castedContext; }
        }

        protected override void AfterContextChanged()
        {
            base.AfterContextChanged();

            try
            {
                castedContext = (T) RawContext;
            }
            catch (InvalidCastException e)
            {
                Debug.LogError($"Context was not of expected type. [expectedType={typeof(T)}, actualType={RawContext.GetType()}]");
            }
        }
    }
}