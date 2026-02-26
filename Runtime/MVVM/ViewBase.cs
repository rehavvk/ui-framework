namespace Rehawk.UIFramework
{
    /// <summary>
    /// Represents the base class for a view that operates with a specific type of model.
    /// This class simplifies the management of the view-model binding by providing a strongly-typed interface
    /// and handling the context lifecycle inherited from <see cref="UIContextControlBase{T}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the view model associated with the view.
    /// </typeparam>
    public abstract class ViewBase<T> : UIContextControlBase<T>
    {
        public T Model => Context;

        public void SetModel(T model)
        {
            SetContext(model);
        }
    }
}