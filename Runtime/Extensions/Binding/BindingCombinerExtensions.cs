namespace Rehawk.UIFramework
{
    public static class BindingCombinerExtensions
    {
        public static Binding CombineByFunction(this Binding binding, MultiValueConvertFunctionDelegate valueConverter)
        {
            return binding.CombineBy(new FunctionCombiner(valueConverter));
        }

        public static Binding CombineByFormat(this Binding binding, string format)
        {
            return binding.CombineBy(new StringFormatCombiner(format));
        }
        
        public static Binding CombineBy(this Binding binding, IValueCombiner valueCombiner)
        {
            MultiBindingStrategy multiBindingStrategy = MultiBindingHelper.ReplaceSourceWithMultiBindingStrategy(binding);
            
            multiBindingStrategy.SetCombiner(valueCombiner);

            return binding;
        }
    }
}