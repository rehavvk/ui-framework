using System;

namespace Rehawk.UIFramework
{
    public interface ICommand
    {
        event Action CanExecuteChanged; 
        
        bool CanExecute(object args);
        void Execute(object args);
    }
}