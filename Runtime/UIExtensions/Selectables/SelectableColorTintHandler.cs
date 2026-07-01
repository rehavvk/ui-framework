using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    public class SelectableColorTintHandler : SelectableStateHandlerBase
    {
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private ColorBlock _colors = ColorBlock.defaultColorBlock;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _colors.fadeDuration = Mathf.Max(_colors.fadeDuration, 0.0f);
            
            if (isActiveAndEnabled)
            {
                StartColorTween(Color.white, true);
            }
            
            base.OnValidate();
        }
#endif

        protected override void HandleStateTransition(SelectionState state, bool instant)
        {
            Color tintColor = GetColorForState(state);
            StartColorTween(tintColor, instant);
        }
        
        private Color GetColorForState(SelectionState state)
        {
            Color tintColor = state switch
            {
                SelectionState.Normal => _colors.normalColor,
                SelectionState.Highlighted => _colors.highlightedColor,
                SelectionState.Pressed => _colors.pressedColor,
                SelectionState.Selected => _colors.selectedColor,
                SelectionState.Disabled => _colors.disabledColor,
                _ => Color.black
            };
            
            return tintColor * _colors.colorMultiplier;
        }
        
        private void StartColorTween(Color targetColor, bool instant)
        {
            if (!_targetGraphic)
                return;

            _targetGraphic.CrossFadeColor(targetColor, instant ? 0f : _colors.fadeDuration, true, true);
        }
    }
}
