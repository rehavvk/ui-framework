using UnityEngine;

using UnityEngine.Scripting.APIUpdating;

namespace Rehawk.UIFramework.UIExtensions
{
    public class SelectableSetActiveHandler : SelectableStateHandlerBase
    {
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private SelectableState _selectableState = SelectableState.Default;
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (isActiveAndEnabled)
            {
                DoStateSwap(false);
            }
            
            base.OnValidate();
        }
#endif
        
        protected override void HandleStateTransition(SelectionState state, bool instant)
        {
            bool isActive;
            
            switch (state)
            {
                case SelectionState.Normal:
                    isActive = _selectableState.NormalState;
                    break;
                case SelectionState.Highlighted:
                    isActive = _selectableState.HighlightedState;
                    break;
                case SelectionState.Pressed:
                    isActive = _selectableState.PressedState;
                    break;
                case SelectionState.Selected:
                    isActive = _selectableState.SelectedState;
                    break;
                case SelectionState.Disabled:
                    isActive = _selectableState.DisabledState;
                    break;
                default:
                    isActive = true;
                    break;
            }
            
            DoStateSwap(isActive);
        }
        
        private void DoStateSwap(bool isActive)
        {
            if (_targetObject == null)
                return;
            
            _targetObject.SetActive(isActive);
        }
    }
}
