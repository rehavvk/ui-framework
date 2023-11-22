using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Rehawk.UIFramework
{
    public class Binding
    {
        private readonly object parent;
        
        private IBindingStrategy sourceStrategy;
        private IBindingStrategy destinationStrategy;
        
        private IValueConverter converter;
        private BindingDirection direction;

        private readonly List<IBindingConnection> connections = new List<IBindingConnection>();

        public event Action<EvaluationDirection> Evaluated;
            
        private Binding(object parent)
        {
            this.parent = parent;
        }

        internal object Parent
        {
            get { return parent; }
        }

        internal IBindingStrategy SourceStrategy
        {
            get { return sourceStrategy; }
        }
        
        internal IBindingStrategy DestinationStrategy
        {
            get { return destinationStrategy; }
        }

        internal IValueConverter Converter
        {
            get { return converter; }
        }

        internal void Release()
        {
            if (sourceStrategy != null)
            {
                sourceStrategy.Release();
            }    
            
            if (destinationStrategy != null)
            {
                destinationStrategy.Release();
            }

            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Release();
            }
        }
        
        internal void SetSource(IBindingStrategy strategy)
        {
            if (sourceStrategy != null)
            {
                sourceStrategy.GotDirty -= OnSourceGotDirty;
            }
            
            sourceStrategy = strategy;
            sourceStrategy.Evaluate();
            
            if (sourceStrategy != null)
            {
                sourceStrategy.GotDirty += OnSourceGotDirty;
            }
        }

        internal void SetDestination(IBindingStrategy strategy)
        {
            if (destinationStrategy != null)
            {
                destinationStrategy.GotDirty -= OnDestinationGotDirty;
            }
            
            destinationStrategy = strategy;
            destinationStrategy.Evaluate();
            
            if (destinationStrategy != null)
            {
                destinationStrategy.GotDirty += OnDestinationGotDirty;
            }
        }
        
        internal void SetConverter(IValueConverter converter)
        {
            this.converter = converter;
        }

        internal void SetDirection(BindingDirection direction)
        {
            this.direction = direction;
        }

        internal void Evaluate()
        {
            Evaluate(EvaluationDirection.Source);
        }

        private void Evaluate(EvaluationDirection direction)
        {
            sourceStrategy.Evaluate();
            destinationStrategy.Evaluate();

            for (int i = 0; i < connections.Count; i++)
            {
                connections[i].Evaluate();
            }
            
            Evaluated?.Invoke(direction);
        }
        
        internal void SourceToDestination()
        {
            object value = sourceStrategy.Get();
            
            if (converter != default)
            {
                value = converter.Convert(value);
            }
            
            destinationStrategy.Set(value);
        }

        private void DestinationToSource()
        {
            object value = destinationStrategy.Get();
            
            if (converter != null)
            {
                value = converter.ConvertBack(value);
            }
            
            sourceStrategy.Set(value);
        }

        internal void ConnectTo<T>(Expression<Func<T>> memberExpression, BindingConnectionDirection direction = BindingConnectionDirection.SourceToDestination)
        {
            var connectedProperty = new MemberConnection(() => parent, MemberPath.Get(memberExpression), direction);
            connectedProperty.Changed += () =>
            {
                OnBindingConnectionChanged(connectedProperty.Direction);
            };
            
            connections.Add(connectedProperty);
        }

        internal void ConnectTo(Func<INotifyPropertyChanged> getContextFunction, string propertyName, BindingConnectionDirection direction = BindingConnectionDirection.SourceToDestination)
        {
            var connectedProperty = new PropertyConnection(getContextFunction, propertyName, direction);
            connectedProperty.Changed += () =>
            {
                OnBindingConnectionChanged(connectedProperty.Direction);
            };
            
            connections.Add(connectedProperty);
        }

        private void OnSourceGotDirty()
        {
            SourceToDestination();
        }

        private void OnDestinationGotDirty()
        {
            if (direction == BindingDirection.TwoWay)
            {
                DestinationToSource();
            }
        }
        
        private void OnBindingConnectionChanged(BindingConnectionDirection direction)
        {
            switch (direction)
            {
                case BindingConnectionDirection.SourceToDestination:
                    Evaluate(EvaluationDirection.Source);
                    SourceToDestination();
                    break;
                case BindingConnectionDirection.DestinationToSource:
                    Evaluate(EvaluationDirection.Destination);
                    DestinationToSource();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static Binding BindMember<T>(object parent, Func<object> getContextFunction, Expression<Func<T>> memberExpression, BindingDirection direction = BindingDirection.OneWay)
        {
            var binding = new Binding(parent);

            binding.SetDestination(new MemberBindingStrategy(getContextFunction, MemberPath.Get(memberExpression)));
            binding.SetDirection(direction);
            
            return binding;
        }

        public static Binding BindProperty(object parent, Func<object> getContextFunction, string propertyName, BindingDirection direction = BindingDirection.OneWay)
        {
            var binding = new Binding(parent);
            
            binding.SetDestination(new ContextPropertyBindingStrategy(getContextFunction, propertyName));
            binding.SetDirection(direction);
            
            return binding;
        }
        
        public static Binding BindContext(object parent, Func<UIContextControlBase> getControlFunction, BindingDirection direction = BindingDirection.OneWay)
        {
            var binding = new Binding(parent);

            binding.SetDestination(new ContextBindingStrategy(getControlFunction));
            binding.SetDirection(direction);

            return binding;
        }
        
        public static Binding BindCallback<T>(object parent, Action<T> setCallback)
        {
            return BindCallback<T>(parent, () => default, setCallback);
        }
        
        public static Binding BindCallback<T>(object parent, Func<T> getFunction, Action<T> setCallback, BindingDirection direction = BindingDirection.OneWay)
        {
            var binding = new Binding(parent);
            
            binding.SetDestination(new CallbackBindingStrategy<T>(getFunction, setCallback));
            binding.SetDirection(direction);
            
            return binding;
        }
    }
}