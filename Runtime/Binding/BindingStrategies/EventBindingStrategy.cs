using System;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Represents a binding strategy that subscribes to a C# event and fires <see cref="GotDirty"/>
    /// each time the event is raised. Use this as a source strategy via <c>ToEvent(...)</c>.
    /// </summary>
    public class EventBindingStrategy : IBindingStrategy
    {
        private bool isSubscribed;
        private bool wasTriggered;

        private readonly Action<Action> subscribe;
        private readonly Action<Action> unsubscribe;

        public event Action GotDirty;

        /// <param name="subscribe">Action that subscribes a handler to the target event, e.g. <c>h => Context.MyEvent += h</c>.</param>
        /// <param name="unsubscribe">Action that unsubscribes a handler from the target event, e.g. <c>h => Context.MyEvent -= h</c>.</param>
        public EventBindingStrategy(Action<Action> subscribe, Action<Action> unsubscribe)
        {
            this.subscribe = subscribe;
            this.unsubscribe = unsubscribe;
        }

        public void Evaluate()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }

            try
            {
                subscribe?.Invoke(OnEventRaised);
                isSubscribed = true;
            }
            catch { }
        }

        public void Release()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }
        }

        /// <summary>
        /// Returns <see cref="Triggered"/> when the event has just fired, then resets. Returns <c>null</c>
        /// on the initial SetDirty pull so the destination callback is not invoked at startup.
        /// </summary>
        public object Get()
        {
            if (!wasTriggered)
                return null;

            wasTriggered = false;
            return Triggered;
        }

        public void Set(object value) { }

        private void OnEventRaised()
        {
            wasTriggered = true;
            GotDirty?.Invoke();
        }
        
        /// <summary>
        /// Sentinel value returned by <see cref="Get"/> when the event has fired.
        /// Destinations can check for this reference to distinguish a real event trigger
        /// from the initial SetDirty pull (which returns <c>null</c>).
        /// </summary>
        internal static readonly object Triggered = new object();
    }

    /// <summary>
    /// Represents a binding strategy that subscribes to a C# event with one argument and fires <see cref="GotDirty"/>
    /// each time the event is raised, forwarding the event argument as the binding value.
    /// Use this as a source strategy via <c>ToEvent&lt;T&gt;(...)</c>.
    /// </summary>
    /// <typeparam name="T">The type of the event argument.</typeparam>
    public class EventBindingStrategy<T> : IBindingStrategy
    {
        private bool isSubscribed;
        private bool wasTriggered;
        private object lastValue;

        private readonly Action<Action<T>> subscribe;
        private readonly Action<Action<T>> unsubscribe;

        public event Action GotDirty;

        /// <param name="subscribe">Action that subscribes a handler to the target event, e.g. <c>h => Context.MyEvent += h</c>.</param>
        /// <param name="unsubscribe">Action that unsubscribes a handler from the target event, e.g. <c>h => Context.MyEvent -= h</c>.</param>
        public EventBindingStrategy(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe)
        {
            this.subscribe = subscribe;
            this.unsubscribe = unsubscribe;
        }

        public void Evaluate()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }

            try
            {
                subscribe?.Invoke(OnEventRaised);
                isSubscribed = true;
            }
            catch { }
        }

        public void Release()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }
        }

        /// <summary>
        /// Returns the last event argument only when the event has just fired, then resets to default.
        /// Returns null on the initial SetDirty pull so the destination callback is not invoked at startup.
        /// </summary>
        public object Get()
        {
            if (!wasTriggered)
                return null;

            wasTriggered = false;
            return lastValue;
        }

        public void Set(object value) { }

        private void OnEventRaised(T arg)
        {
            wasTriggered = true;
            lastValue = arg;
            GotDirty?.Invoke();
        }
    }

    /// <summary>
    /// Represents a binding strategy that subscribes to a C# event with two arguments and fires <see cref="GotDirty"/>
    /// each time the event is raised, forwarding the arguments as a <see cref="ValueTuple{T1,T2}"/>.
    /// Use this as a source strategy via <c>ToEvent&lt;T1,T2&gt;(...)</c>.
    /// </summary>
    public class EventBindingStrategy<T1, T2> : IBindingStrategy
    {
        private bool isSubscribed;
        private bool wasTriggered;
        private object lastValue;

        private readonly Action<Action<T1, T2>> subscribe;
        private readonly Action<Action<T1, T2>> unsubscribe;

        public event Action GotDirty;

        public EventBindingStrategy(Action<Action<T1, T2>> subscribe, Action<Action<T1, T2>> unsubscribe)
        {
            this.subscribe = subscribe;
            this.unsubscribe = unsubscribe;
        }

        public void Evaluate()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }

            try
            {
                subscribe?.Invoke(OnEventRaised);
                isSubscribed = true;
            }
            catch { }
        }

        public void Release()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }
        }

        public object Get()
        {
            if (!wasTriggered)
                return null;

            wasTriggered = false;
            return lastValue;
        }

        public void Set(object value) { }

        private void OnEventRaised(T1 arg1, T2 arg2)
        {
            wasTriggered = true;
            lastValue = (arg1, arg2);
            GotDirty?.Invoke();
        }
    }

    /// <summary>
    /// Represents a binding strategy that subscribes to a C# event with three arguments and fires <see cref="GotDirty"/>
    /// each time the event is raised, forwarding the arguments as a <see cref="ValueTuple{T1,T2,T3}"/>.
    /// Use this as a source strategy via <c>ToEvent&lt;T1,T2,T3&gt;(...)</c>.
    /// </summary>
    public class EventBindingStrategy<T1, T2, T3> : IBindingStrategy
    {
        private bool isSubscribed;
        private bool wasTriggered;
        private object lastValue;

        private readonly Action<Action<T1, T2, T3>> subscribe;
        private readonly Action<Action<T1, T2, T3>> unsubscribe;

        public event Action GotDirty;

        public EventBindingStrategy(Action<Action<T1, T2, T3>> subscribe, Action<Action<T1, T2, T3>> unsubscribe)
        {
            this.subscribe = subscribe;
            this.unsubscribe = unsubscribe;
        }

        public void Evaluate()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }

            try
            {
                subscribe?.Invoke(OnEventRaised);
                isSubscribed = true;
            }
            catch { }
        }

        public void Release()
        {
            if (isSubscribed)
            {
                try { unsubscribe?.Invoke(OnEventRaised); } catch { }
                isSubscribed = false;
            }
        }

        public object Get()
        {
            if (!wasTriggered)
                return null;

            wasTriggered = false;
            return lastValue;
        }

        public void Set(object value) { }

        private void OnEventRaised(T1 arg1, T2 arg2, T3 arg3)
        {
            wasTriggered = true;
            lastValue = (arg1, arg2, arg3);
            GotDirty?.Invoke();
        }
    }
}
