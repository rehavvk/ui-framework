using System;

namespace Rehawk.UIFramework
{
    [Serializable]
    public abstract class SliceBase<T> : UIVirtualContextControlBase<T>
    {
        public T Model => Context;

        public void SetModel(T model)
        {
            SetContext(model);
        }
    }
}