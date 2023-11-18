using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace Rehawk.UIFramework
{
    public class ContextPropertyBindingStrategy : IBindingStrategy
    {
        private readonly Func<object> getContextFunction;
        private readonly string propertyName;
        
        private PropertyInfo propertyInfo;

        private IValueConverter converter;
        
        private object context;
        private object value;
        
        public event Action GotDirty;

        public ContextPropertyBindingStrategy(Func<object> getContextFunction, string propertyName)
        {
            this.getContextFunction = getContextFunction;
            this.propertyName = propertyName;
        }

        public void SetConverter(IValueConverter converter)
        {
            this.converter = converter;
        }

        public void Evaluate()
        {
            UnLinkFromEvents();
            
            context = getContextFunction?.Invoke();
            
            if (context != null && !string.IsNullOrEmpty(propertyName))
            {
                propertyInfo = context.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            }

            value = Get();

            LinkToEvents();
        }
        
        public void Release()
        {
            UnLinkFromEvents();
        }

        public object Get()
        {
            object result = context;
            
            if (context != null && propertyInfo != null)
            {
                result = propertyInfo.GetValue(context, null);
            }

            if (converter != default)
            {
                result = converter.Convert(result);
            }

            return result;
        }

        public void Set(object value)
        {
            if (converter != default)
            {
                value = converter.ConvertBack(value);
            }

            if (context != null && propertyInfo != null)
            {
                // Built in converter for strings.
                if (propertyInfo.PropertyType == typeof(string) && value != null)
                {
                    value = value.ToString();
                }
                
                propertyInfo.SetValue(context, value);
            }
            else if (context is UIContextControlBase contextNode)
            {
                contextNode.SetContext(value);
            }
        }
        
        private void LinkToEvents()
        {
            if (value is INotifyCollectionChanged valueNotifyCollectionChanged)
            {
                valueNotifyCollectionChanged.CollectionChanged += OnValueCollectionChanged;
            }
            else if (value is INotifyPropertyChanged valueNotifyPropertyChanged)
            {
                valueNotifyPropertyChanged.PropertyChanged += OnValuePropertyChanged;
            }
            
            if (context is INotifyCollectionChanged contextNotifyCollectionChanged)
            {
                contextNotifyCollectionChanged.CollectionChanged += OnContextCollectionChanged;
            }
            else if (context is INotifyPropertyChanged contextNotifyPropertyChanged)
            {
                contextNotifyPropertyChanged.PropertyChanged += OnContextPropertyChanged;
            }
        }

        private void UnLinkFromEvents()
        {
            if (value is INotifyCollectionChanged valueNotifyCollectionChanged)
            {
                valueNotifyCollectionChanged.CollectionChanged -= OnValueCollectionChanged;
            }
            else if (value is INotifyPropertyChanged valueNotifyPropertyChanged)
            {
                valueNotifyPropertyChanged.PropertyChanged -= OnValuePropertyChanged;
            }

            if (context is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged -= OnContextCollectionChanged;
            }
            
            if (context is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= OnContextPropertyChanged;
            }
        }

        private void OnValueCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GotDirty?.Invoke();
        }

        private void OnValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GotDirty?.Invoke();
        }
        
        private void OnContextCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            GotDirty?.Invoke();
        }

        private void OnContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(propertyName) || e.PropertyName == propertyName)
            {
                GotDirty?.Invoke();
            }
        }
    }
}