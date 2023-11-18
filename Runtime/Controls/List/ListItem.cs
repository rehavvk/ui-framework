namespace Rehawk.UIFramework
{
    public struct ListItem
    {
        public int Index { get; }
        public object Data { get; }

        public ListItem(int index, object data)
        {
            Index = index;
            Data = data;
        }
    }
}