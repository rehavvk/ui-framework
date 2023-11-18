namespace Rehawk.UIFramework
{
    internal static class MultiBindingHelper
    {
        internal static void AddSourceStrategy(Binding binding, IBindingStrategy bindingStrategy)
        {
            if (binding.SourceStrategy == null)
            {
                binding.SetSource(bindingStrategy);
            }
            else if (binding.SourceStrategy is MultiBindingStrategy multiBindingStrategy)
            {
                multiBindingStrategy.AddBindingStrategy(bindingStrategy);
            }
            else
            {
                IBindingStrategy previousSourceStrategy = binding.SourceStrategy;
                
                multiBindingStrategy = new MultiBindingStrategy();
                multiBindingStrategy.AddBindingStrategy(previousSourceStrategy);
                multiBindingStrategy.AddBindingStrategy(bindingStrategy);

                binding.SetSource(multiBindingStrategy);
            }
        }

        internal static MultiBindingStrategy ReplaceSourceWithMultiBindingStrategy(Binding binding)
        {
            if (binding.SourceStrategy is MultiBindingStrategy multiBindingStrategy)
            {
                // Do nothing.
            }
            else if (binding.SourceStrategy != null)
            {
                IBindingStrategy previousSourceStrategy = binding.SourceStrategy;
                multiBindingStrategy = new MultiBindingStrategy();
                multiBindingStrategy.AddBindingStrategy(previousSourceStrategy);
                
                binding.SetSource(multiBindingStrategy);
            }
            else
            {
                multiBindingStrategy = new MultiBindingStrategy();
                
                binding.SetSource(multiBindingStrategy);
            }

            return multiBindingStrategy;
        }
    }
}