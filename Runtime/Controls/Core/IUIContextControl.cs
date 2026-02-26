using System;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Represents a UI control that manages a context object and provides methods for setting, retrieving, and managing the context.
    /// </summary>
    public interface IUIContextControl : IUIControl
    {
        /// <summary>
        /// Occurs when the context associated with this control is changed.
        /// </summary>
        /// <remarks>
        /// This event is triggered whenever the context object of the control is modified,
        /// typically through methods such as <c>SetContext</c> or <c>ClearContext</c>.
        /// Subscribing to this event allows responding to context updates or changes.
        /// </remarks>
        event Action ContextChanged;

        /// <summary>
        /// Gets a value indicating whether a context object is currently assigned to the control.
        /// </summary>
        /// <remarks>
        /// This property is used to determine the presence of an active context within the control.
        /// It returns <c>true</c> if a context is assigned; otherwise, <c>false</c>.
        /// </remarks>
        bool HasContext { get; }

        /// <summary>
        /// Gets the raw context object associated with the control.
        /// </summary>
        /// <remarks>
        /// The raw context represents the internal object used by the control. It can be any type of object and serves
        /// as the underlying data or state managed by the context control. This property facilitates access to the
        /// unmanaged or untyped context in scenarios where type-specific access is not required or applicable.
        /// </remarks>
        object RawContext { get; }

        /// <summary>
        /// Retrieves the current context object if it matches the specified type.
        /// </summary>
        /// <typeparam name="T">The expected type of the context object.</typeparam>
        /// <returns>The context object cast to the specified type, or the default value if the context is not of the specified type.</returns>
        T GetContext<T>();

        /// <summary>
        /// Attempts to retrieve the current context object if it matches the specified type.
        /// </summary>
        /// <typeparam name="T">The expected type of the context object.</typeparam>
        /// <param name="context">When this method returns, contains the context object cast to the specified type if the cast is successful; otherwise, contains the default value of the type.</param>
        /// <returns>True if the context is successfully cast to the specified type; otherwise, false.</returns>
        bool TryGetContext<T>(out T context);

        /// <summary>
        /// Sets the context object for this control to the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the context object to be set.</typeparam>
        /// <param name="context">The new context object to assign to this control.</param>
        void SetContext<T>(T context);

        /// <summary>
        /// Clears the context object for this control by setting it to null.
        /// </summary>
        void ClearContext();
    }
}