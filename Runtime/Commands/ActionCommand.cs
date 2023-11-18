using System;

namespace Rehawk.UIFramework
{
    public delegate void CommandActionDelegate(object args);

    public class ActionCommand : ICommand
    {
        private readonly CommandActionDelegate commandAction;
        private bool isExecutable;

        public event Action CanExecuteChanged;
        
        public ActionCommand(CommandActionDelegate commandAction, bool isExecutable = true)
        {
            this.commandAction = commandAction;
            this.isExecutable = isExecutable;
        }
        
        public bool IsExecutable
        {
            get { return isExecutable; }
            set
            {
                if (value != isExecutable)
                {
                    isExecutable = value;
                    CanExecuteChanged?.Invoke();
                }
            } 
        }
        
        public bool CanExecute(object args)
        {
            return IsExecutable;
        }

        public void Execute(object args)
        {
            commandAction.Invoke(args);
        }
    }
}