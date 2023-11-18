namespace Rehawk.UIFramework
{
    public class InvertedBoolConverter : BoolConverter
    {
        public override object Convert(object value)
        {
            value = base.Convert(value);
            
            if (value != null)
            {
                return !((bool)value);
            }
                    
            // Null is processed like false
                    
            return true;
        }
    }
}