using System;

namespace Rehawk.UIFramework.UIExtensions
{
    [Serializable]
    public struct SelectableState
    {
        public bool NormalState;
        public bool HighlightedState;
        public bool PressedState;
        public bool SelectedState;
        public bool DisabledState;
            
        public static SelectableState Default;

        static SelectableState()
        {
            Default = new SelectableState
                                 {
                                     NormalState = true,
                                     HighlightedState = true,
                                     PressedState = true,
                                     SelectedState = true,
                                     DisabledState = false
                                 };
        }
    }
}