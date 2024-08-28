using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public abstract class UIContextControlBase : UIControlBase
    {
        private object context;
        
        public event Action ContextChanged;

        public bool HasContext
        {
            get { return context != default; }
        }

        public object RawContext
        {
            get { return context; }
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
            
            this.context = context;

            AfterContextChanged();

            SetDirty();

            ContextChanged?.Invoke();
        }

        public void ClearContext()
        {
            SetContext<object>(null);
        }

        /// <summary>
        /// Is called before the context is switched to another instance or cleared.
        /// </summary>
        protected virtual void BeforeContextChanged() {}
        
        /// <summary>
        /// Is called after the context is switched to another instance or cleared.
        /// </summary>
        protected virtual void AfterContextChanged() {}
    }
    
    public abstract class UIContextControlBase<T> : UIContextControlBase
    {
        private T castedContext;

        public T Context
        {
            get { return castedContext; }
        }

        public Type ContextBaseType
        {
            get { return typeof(T); }
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