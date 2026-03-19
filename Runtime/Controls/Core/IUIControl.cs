using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Rehawk.UIFramework
{
    public interface IUIControl : INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a binding between a property or field represented by the given expression and the current instance.
        /// The binding direction determines whether changes are tracked one-way or two-way.
        /// </summary>
        /// <typeparam name="T">The type of the property or field being bound.</typeparam>
        /// <param name="memberExpression">An expression representing the property or field to be bound.</param>
        /// <param name="direction">The direction of the binding. Defaults to one-way binding.</param>
        /// <returns>Returns a <see cref="Binding"/> object representing the created binding.</returns>
        Binding Bind<T>(Expression<Func<T>> memberExpression, BindingDirection direction = BindingDirection.OneWay);

        /// <summary>
        /// Creates a binding to a property specified by its name and resolves the property context dynamically through the provided context function.
        /// The binding allows tracking and synchronization of property changes based on the specified binding direction.
        /// </summary>
        /// <param name="getContext">A function providing the context object that contains the property to bind to.</param>
        /// <param name="propertyName">The name of the property to bind.</param>
        /// <param name="direction">The direction of the binding (e.g., OneWay or TwoWay). Defaults to OneWay binding.</param>
        /// <returns>Returns a <see cref="Binding"/> object representing the created binding.</returns>
        Binding BindProperty(Func<object> getContext, string propertyName,
            BindingDirection direction = BindingDirection.OneWay);

        /// <summary>
        /// Binds the control context provided by the given callback function to the current instance,
        /// enabling data synchronization between the control context and the current instance based on the specified binding direction.
        /// </summary>
        /// <param name="getControlCallback">A function returning the control context to be bound.</param>
        /// <param name="direction">The direction of the binding, determining whether the synchronization is one-way or two-way. Defaults to one-way.</param>
        /// <returns>Returns a <see cref="Binding"/> object representing the created binding.</returns>
        Binding BindContext(Func<UIContextControlBase> getControlCallback,
            BindingDirection direction = BindingDirection.OneWay);

        /// <summary>
        /// Creates a binding with a C# event as the source. Chain with <c>.To()</c> or <c>.ToCallback()</c>
        /// to specify the destination. The destination is only invoked when the event actually fires,
        /// not on the initial SetDirty pull.
        /// </summary>
        /// <param name="subscribe">Action that adds a handler to the target event, e.g. <c>h => Context.MyEvent += h</c>.</param>
        /// <param name="unsubscribe">Action that removes the handler from the target event, e.g. <c>h => Context.MyEvent -= h</c>.</param>
        /// <returns>Returns a <see cref="Binding"/> object representing the created binding.</returns>
        Binding BindEvent(Action<Action> subscribe, Action<Action> unsubscribe);

        /// <summary>
        /// Creates a binding with a C# event as the source, forwarding the event argument as the binding value.
        /// Chain with <c>.To()</c> or <c>.ToCallback()</c> to specify the destination. The destination is only
        /// invoked when the event actually fires, not on the initial SetDirty pull.
        /// </summary>
        /// <typeparam name="T">The type of the event argument.</typeparam>
        /// <param name="subscribe">Action that adds a handler to the target event, e.g. <c>h => Context.MyEvent += h</c>.</param>
        /// <param name="unsubscribe">Action that removes the handler from the target event, e.g. <c>h => Context.MyEvent -= h</c>.</param>
        /// <returns>Returns a <see cref="Binding"/> object representing the created binding.</returns>
        Binding BindEvent<T>(Action<Action<T>> subscribe, Action<Action<T>> unsubscribe);

        Binding BindEvent<T1, T2>(Action<Action<T1, T2>> subscribe, Action<Action<T1, T2>> unsubscribe);

        Binding BindEvent<T1, T2, T3>(Action<Action<T1, T2, T3>> subscribe, Action<Action<T1, T2, T3>> unsubscribe);

        /// <summary>
        /// Creates a binding whose destination invokes a parameterless callback.
        /// Intended for use with <c>ToEvent(...)</c> to react to events without a value.
        /// </summary>
        /// <param name="callback">The parameterless callback to invoke when the source fires.</param>
        /// <returns>Returns a <see cref="Binding"/> object representing the created binding.</returns>
        Binding BindCallback(Action callback);

        /// <summary>
        /// Creates a binding for a callback, allowing a value to be set through the specified callback action.
        /// This binding enables interaction between the current instance and external components or data.
        /// </summary>
        /// <typeparam name="T">The type of the value being bound and passed to the callback.</typeparam>
        /// <param name="setCallback">The callback action responsible for setting the value in the target binding.</param>
        /// <returns>Returns a <see cref="Binding"/> object representing the created binding.</returns>
        Binding BindCallback<T>(Action<T> setCallback);

        /// <summary>
        /// Creates a binding where a callback is triggered upon a change, enabling flexible handling of updates
        /// between the control and the bound data.
        /// </summary>
        /// <typeparam name="T">The type of the data value used in the binding.</typeparam>
        /// <param name="getCallback">A function used to retrieve the current value of the bound data.</param>
        /// <param name="setCallback">An action used to set or update the value of the bound data.</param>
        /// <returns>Returns a <see cref="Binding"/> instance representing the created callback binding.</returns>
        Binding BindCallback<T>(Func<T> getCallback, Action<T> setCallback);

        /// <summary>
        /// Marks the current control and its bindings as needing to be refreshed or updated.
        /// This method is commonly used to invalidate the state of the bindings, ensuring they
        /// are re-evaluated and properly synchronized during the next update cycle.
        /// </summary>
        void SetDirty();
    }
}