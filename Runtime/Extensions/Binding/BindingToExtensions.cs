using System;
using System.Linq.Expressions;

namespace Rehawk.UIFramework
{
    public static class BindingToExtensions
    {
        /// <summary>
        /// Binds the supplied member expression to the current binding with an optional converter.
        /// </summary>
        /// <param name="binding">The binding instance to set the member expression and converter on.</param>
        /// <param name="memberExpression">The lambda expression defining the property or member to bind to.</param>
        /// <param name="converter">An optional value converter to transform the member's value during binding.</param>
        /// <typeparam name="T">The type of the member being bound.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding To<T>(this Binding binding, Expression<Func<T>> memberExpression,
                                    IValueConverter converter = null)
        {
            var bindingStrategy = new MemberBindingStrategy(() => binding.Parent, MemberPath.Get(memberExpression));
            bindingStrategy.SetConverter(converter);
            
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            
            return binding;
        }

        /// <summary>
        /// Binds the specified member expression to the current binding using a custom conversion function.
        /// </summary>
        /// <param name="binding">The binding instance to configure with the member expression and conversion.</param>
        /// <param name="memberExpression">The lambda expression indicating the property or member to bind to.</param>
        /// <param name="converterFunction">A delegate function that performs the value conversion during binding.</param>
        /// <typeparam name="T">The type of the member being bound.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding To<T>(this Binding binding, Expression<Func<T>> memberExpression,
                                    ValueConvertFunctionDelegate<T> converterFunction)
        {
            var converter = FunctionConverter.CreateTyped(converterFunction);

            var bindingStrategy = new MemberBindingStrategy(() => binding.Parent, MemberPath.Get(memberExpression));
            bindingStrategy.SetConverter(converter);
            
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            
            return binding;
        }

        /// <summary>
        /// Binds the specified context property to the current binding with an optional value converter.
        /// </summary>
        /// <param name="binding">The binding instance to configure.</param>
        /// <param name="getContextFunction">A function that retrieves the context object containing the property to bind to.</param>
        /// <param name="propertyName">The name of the property in the context to bind to.</param>
        /// <param name="converter">An optional value converter used to transform the property value during binding.</param>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToProperty(this Binding binding, Func<object> getContextFunction, string propertyName,
                                         IValueConverter converter = null)
        {
            var bindingStrategy = new ContextPropertyBindingStrategy(getContextFunction, propertyName);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Binds a property to the provided context function and property name, using an optional converter function.
        /// </summary>
        /// <param name="binding">The binding instance to configure.</param>
        /// <param name="getContextFunction">A function to retrieve the context object used for the binding.</param>
        /// <param name="propertyName">The name of the property to bind within the context object.</param>
        /// <param name="converterFunction">A function used to convert values during the binding process.</param>
        /// <returns>The configured binding instance.</returns>
        public static Binding ToProperty(this Binding binding, Func<object> getContextFunction, string propertyName,
                                         ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new ContextPropertyBindingStrategy(getContextFunction, propertyName);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Binds the specified binding instance to a context control provided by the given function,
        /// with an optional value converter to transform the data during binding.
        /// </summary>
        /// <param name="binding">The binding instance to modify.</param>
        /// <param name="getControlFunction">A function that retrieves the target context control to bind to.</param>
        /// <param name="converter">An optional value converter applied during data transformation.</param>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToContext(this Binding binding, Func<UIContextControlBase> getControlFunction,
                                        IValueConverter converter = null)
        {
            var bindingStrategy = new ContextBindingStrategy(getControlFunction);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Binds the supplied function that retrieves a <see cref="UIContextControlBase"/> to the current binding with an optional converter function.
        /// </summary>
        /// <param name="binding">The binding instance to configure the context binding.</param>
        /// <param name="getControlFunction">A function that returns the desired <see cref="UIContextControlBase"/> for the binding.</param>
        /// <param name="converterFunction">A delegate for a custom converter function that transforms the bound value during binding.</param>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToContext(this Binding binding, Func<UIContextControlBase> getControlFunction,
                                        ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new ContextBindingStrategy(getControlFunction);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Sets a parameterless action as the destination of the binding. Intended for use with
        /// <c>BindEvent(...)</c> to react to events without receiving a value.
        /// </summary>
        /// <param name="binding">The binding instance to configure.</param>
        /// <param name="callback">The action to invoke when the source changes.</param>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToCallback(this Binding binding, Action callback)
        {
            binding.SetDestination(new CallbackBindingStrategy<object>(() => null, _ => callback.Invoke()));
            return binding;
        }

        /// <summary>
        /// Binds the specified callback function to the current binding, optionally using a value converter.
        /// </summary>
        /// <param name="binding">The binding instance to set the callback function on.</param>
        /// <param name="getFunction">The function to retrieve the value to bind.</param>
        /// <param name="converter">An optional value converter to transform the value during binding.</param>
        /// <typeparam name="T">The type of the value being retrieved by the function.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction, IValueConverter converter = null)
        {
            return binding.ToCallback(getFunction, _ => { }, converter);
        }

        /// <summary>
        /// Binds the provided callback function to the current binding with an optional converter.
        /// </summary>
        /// <param name="binding">The binding instance to configure the callback on.</param>
        /// <param name="getFunction">The function used to retrieve the source value for binding.</param>
        /// <param name="converter">An optional value converter to transform the value during binding.</param>
        /// <typeparam name="T">The type of the value returned by the callback function.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction,
                                            ValueConvertFunctionDelegate converterFunction)
        {
            return binding.ToCallback(getFunction, _ => { }, converterFunction);
        }

        /// <summary>
        /// Binds the current binding to a callback function for getting and setting values, with an optional value converter.
        /// </summary>
        /// <param name="binding">The binding instance to modify with the callback strategy.</param>
        /// <param name="getFunction">The function to retrieve the binding value.</param>
        /// <param name="setCallback">The action to update the value when the binding changes.</param>
        /// <param name="converter">An optional value converter to transform the binding value.</param>
        /// <typeparam name="T">The type of the value being handled by the callback.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction, Action<T> setCallback,
                                            IValueConverter converter = null)
        {
            var bindingStrategy = new CallbackBindingStrategy<T>(getFunction, setCallback);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Binds to a callback using the specified getter and setter functions, applying an optional value converter function.
        /// </summary>
        /// <param name="binding">The binding instance to configure the callback strategy on.</param>
        /// <param name="getFunction">A function that retrieves the value from the source.</param>
        /// <param name="setCallback">A callback action that updates the value at the source.</param>
        /// <param name="converterFunction">An optional value conversion delegate to transform values during the binding process.</param>
        /// <typeparam name="T">The type of the value being bound.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToCallback<T>(this Binding binding, Func<T> getFunction, Action<T> setCallback,
                                            ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new CallbackBindingStrategy<T>(getFunction, setCallback);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Binds the provided static value to the current binding with an optional value converter.
        /// </summary>
        /// <param name="binding">The binding instance to which the static value is applied.</param>
        /// <param name="value">The static value to bind.</param>
        /// <param name="converter">An optional value converter to transform the static value during binding.</param>
        /// <typeparam name="T">The type of the value being bound.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToValue<T>(this Binding binding, T value, IValueConverter converter = null)
        {
            var bindingStrategy = new StaticValueBindingStrategy(value);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Binds the given value to the current binding with an optional function-based converter.
        /// </summary>
        /// <param name="binding">The binding instance to associate the value and converter with.</param>
        /// <param name="value">The static value to bind to.</param>
        /// <param name="converterFunction">A delegate function for value conversion, applied during the binding process.</param>
        /// <typeparam name="T">The type of the value being bound.</typeparam>
        /// <returns>The modified binding instance configured with the provided value and converter function.</returns>
        public static Binding ToValue<T>(this Binding binding, T value, ValueConvertFunctionDelegate converterFunction)
        {
            var converter = new FunctionConverter(converterFunction);

            var bindingStrategy = new StaticValueBindingStrategy(value);
            bindingStrategy.SetConverter(converter);

            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Adds a custom binding strategy to the given binding instance.
        /// </summary>
        /// <param name="binding">The binding instance that the custom strategy will be added to.</param>
        /// <param name="bindingStrategy">The custom binding strategy to be applied to the binding instance.</param>
        /// <returns>The modified binding instance with the custom strategy applied.</returns>
        public static Binding ToCustom(this Binding binding, IBindingStrategy bindingStrategy)
        {
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);

            return binding;
        }

        /// <summary>
        /// Binds the current binding to a C# event. The destination callback is invoked each time the event fires.
        /// </summary>
        /// <example>
        /// <code>
        /// BindCallback(() => OnDamaged())
        ///     .ToEvent(
        ///         h => Context.Damaged += h,
        ///         h => Context.Damaged -= h);
        /// </code>
        /// </example>
        /// <param name="binding">The binding instance to configure.</param>
        /// <param name="subscribe">Action that adds a handler to the target event.</param>
        /// <param name="unsubscribe">Action that removes the handler from the target event.</param>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToEvent(this Binding binding, Action<Action> subscribe, Action<Action> unsubscribe)
        {
            binding.SetEventSource();

            var bindingStrategy = new EventBindingStrategy(subscribe, unsubscribe);
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            return binding;
        }

        /// <summary>
        /// Binds the current binding to a C# event with one argument. The destination callback is invoked with
        /// the event argument each time the event fires.
        /// </summary>
        /// <example>
        /// <code>
        /// BindCallback&lt;int&gt;(damage => ShowDamage(damage))
        ///     .ToEvent&lt;int&gt;(
        ///         h => Context.Damaged += h,
        ///         h => Context.Damaged -= h);
        /// </code>
        /// </example>
        /// <param name="binding">The binding instance to configure.</param>
        /// <param name="subscribe">Action that adds a handler to the target event.</param>
        /// <param name="unsubscribe">Action that removes the handler from the target event.</param>
        /// <typeparam name="T">The type of the event argument.</typeparam>
        /// <returns>The modified binding instance.</returns>
        public static Binding ToEvent<T>(this Binding binding, Action<Action<T>> subscribe, Action<Action<T>> unsubscribe)
        {
            binding.SetEventSource();

            var bindingStrategy = new EventBindingStrategy<T>(subscribe, unsubscribe);
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            return binding;
        }

        public static Binding ToEvent<T1, T2>(this Binding binding, Action<Action<T1, T2>> subscribe, Action<Action<T1, T2>> unsubscribe)
        {
            binding.SetEventSource();

            var bindingStrategy = new EventBindingStrategy<T1, T2>(subscribe, unsubscribe);
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            return binding;
        }

        public static Binding ToEvent<T1, T2, T3>(this Binding binding, Action<Action<T1, T2, T3>> subscribe, Action<Action<T1, T2, T3>> unsubscribe)
        {
            binding.SetEventSource();

            var bindingStrategy = new EventBindingStrategy<T1, T2, T3>(subscribe, unsubscribe);
            MultiBindingHelper.AddSourceStrategy(binding, bindingStrategy);
            return binding;
        }

    }
}