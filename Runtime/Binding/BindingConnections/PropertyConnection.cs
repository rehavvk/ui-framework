using System;
using System.ComponentModel;

namespace Rehawk.UIFramework
{
    internal class PropertyConnection : IBindingConnection
    {
        private readonly Func<INotifyPropertyChanged> getContextFunction;
        private readonly string propertyName;
        private readonly BindingConnectionDirection direction;

        private INotifyPropertyChanged context;
        
        public event Action Changed;
            
        public PropertyConnection(Func<INotifyPropertyChanged> getContextFunction, string propertyName, BindingConnectionDirection direction)
        {
            this.getContextFunction = getContextFunction;
            this.propertyName = propertyName;
            this.direction = direction;
        }

        ~PropertyConnection()
        {
            context.PropertyChanged -= OnContextPropertyChanged;
        }

        public BindingConnectionDirection Direction
        {
            get { return direction; }
        }

        public void Evaluate()
        {
            if (context != null)
            {
                context.PropertyChanged -= OnContextPropertyChanged;
            }
            
            context = getContextFunction?.Invoke();

            if (context != null)
            {
                context.PropertyChanged += OnContextPropertyChanged;
            }
        }

        public void Release() { }
        
        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == propertyName)
            {
                Changed?.Invoke();
            }
        }
    }
}