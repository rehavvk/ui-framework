namespace Rehawk.UIFramework
{
    public static class BindingTagsExtensions
    {
        public static Binding WithTag(this Binding binding, params string[] tags)
        {
            binding.AddTags(tags);
            return binding;
        }
    }
}