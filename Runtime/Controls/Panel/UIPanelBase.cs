using System;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Represents the base class for all UI panels.
    /// This abstract class provides common functionality for managing the visibility
    /// and hierarchical relationship of UI panels. It provides methods to control
    /// a panel's visibility and define its parent relationship within the UI structure.
    /// </summary>
    public abstract class UIPanelBase : UIElementBase
    {
        /// <summary>
        /// Event triggered when a <see cref="UIPanel"/> becomes visible.
        /// This event is invoked as part of the visibility management system.
        /// It provides a notification mechanism to react to a <see cref="UIPanel"/> transitioning to a visible state.
        /// Subscribers to this event can execute additional logic when a panel's visibility changes to "visible".
        /// </summary>
        public virtual event Action<UIPanel> BecameVisible;

        /// <summary>
        /// Event triggered when a <see cref="UIPanel"/> becomes invisible.
        /// This event is invoked as part of the visibility management system.
        /// It provides a notification mechanism to react to a <see cref="UIPanel"/> transitioning to an invisible state.
        /// Subscribers to this event can execute additional logic when a panel's visibility changes to "invisible".
        /// </summary>
        public virtual event Action<UIPanel> BecameInvisible;

        /// <summary>
        /// Gets the parent <see cref="UIPanel"/> of the current panel within the UI hierarchy.
        /// This property represents the immediate parent panel that encompasses the current panel,
        /// allowing for hierarchical relationships among UI panels. It is particularly useful
        /// for managing visibility constraints or relationships between panels.
        /// </summary>
        public abstract UIPanel Parent { get; }

        /// <summary>
        /// Sets the visibility of the panel.
        /// </summary>
        /// <param name="visible">A boolean value indicating whether the panel should be visible
        /// (true) or hidden (false).</param>
        /// <param name="instant">A boolean value indicating whether the panel should change it's state instant
        /// or with whatever duration the visibility strategy works.</param>
        public abstract void SetVisible(bool visible, bool instant = false);

        /// <summary>
        /// Toggles the visibility state of the panel. If the panel is currently visible,
        /// it will be hidden. If the panel is currently hidden, it will be made visible.
        /// </summary>
        public abstract void ToggleVisible();
    }
}