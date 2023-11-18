namespace Rehawk.UIFramework
{
    internal static class MultiConverterHelper
    {
        internal static void AddConverter(Binding binding, IValueConverter converter)
        {
            if (binding.Converter == null)
            {
                binding.SetConverter(converter);
            }
            else if (binding.Converter is MultiConverter multiConverter)
            {
                multiConverter.AddConverter(converter);
            }
            else
            {
                IValueConverter previousConverter = binding.Converter;
                
                multiConverter = new MultiConverter();
                multiConverter.AddConverter(previousConverter);
                multiConverter.AddConverter(converter);

                binding.SetConverter(multiConverter);
            }
        }
    }
}