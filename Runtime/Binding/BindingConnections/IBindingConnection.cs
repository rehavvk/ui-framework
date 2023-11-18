using System;

namespace Rehawk.UIFramework
{
    public interface IBindingConnection
    {
        event Action Changed;
        
        BindingConnectionDirection Direction { get; }
        
        void Evaluate();
        void Release();
    }
}