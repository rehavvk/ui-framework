namespace Rehawk.UIFramework
{
    public abstract class ViewBase<T> : UIContextControlBase<T>
    {
        public T ViewModel => Context;

        public void SetViewModel(T viewModel)
        {
            SetContext(viewModel);
        }
    }
}