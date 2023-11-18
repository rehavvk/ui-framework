namespace Rehawk.UIFramework
{
    public interface IValueCombiner
    {
        object Combine(object[] values);
        object[] Divide(object value);
    }
}