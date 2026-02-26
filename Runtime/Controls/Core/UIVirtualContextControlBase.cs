using System;
using UnityEngine;

namespace Rehawk.UIFramework
{
    [Serializable]
    public abstract class UIVirtualContextControlBase : UIVirtualControlBase, IUIContextControl
    {
        public event Action ContextChanged;

        public bool HasContext => RawContext != null;

        public object RawContext { get; private set; }

        public T GetContext<T>()
        {
            if (RawContext is T castedContext)
            {
                return castedContext;
            }

            return default;
        }

        public bool TryGetContext<T>(out T context)
        {
            if (RawContext is T castedValue)
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

            RawContext = context;

            AfterContextChanged();

            SetDirty();

            ContextChanged?.Invoke();
        }

        public void ClearContext()
        {
            SetContext<object>(null);
        }

        /// <summary>
        /// Invoked before the current context is changed. This method provides a hook for derived classes
        /// to implement logic that needs to occur before modifying the context object.
        /// </summary>
        protected virtual void BeforeContextChanged() {}

        /// <summary>
        /// Executed after the context object has been updated. This method serves as a lifecycle hook,
        /// allowing derived classes to handle any necessary operations or adjustments in response to changes in the context.
        /// Can be overridden to implement custom behavior after the context is changed.
        /// </summary>
        protected virtual void AfterContextChanged() {}
    }
    
    [Serializable]
    public abstract class UIVirtualContextControlBase<T> : UIVirtualContextControlBase, IUIContextControl<T>
    {
        public T Context { get; private set; }

        public Type ContextBaseType => typeof(T);

        protected override void AfterContextChanged()
        {
            base.AfterContextChanged();

            try
            {
                Context = (T) RawContext;
            }
            catch (InvalidCastException)
            {
                Debug.LogError($"Context was not of expected type. [expectedType={typeof(T)}, actualType={RawContext.GetType()}]");
            }
        }
    }
}