using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    public class SelectableColorSwapHandler : SelectableStateHandlerBase
    {
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private ColorBlock _colors = ColorBlock.defaultColorBlock;
        [SerializeField] private AnimationCurve _easing = AnimationCurve.EaseInOut(0, 0, 1, 1);
        
        private Coroutine _currentTween;
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _colors.fadeDuration = Mathf.Max(_colors.fadeDuration, 0.0f);
            
            if (isActiveAndEnabled && _targetGraphic)
            {
                _targetGraphic.color = GetColorForState(SelectionState.Normal);
            }

            base.OnValidate();
        }
#endif

        protected override void HandleStateTransition(SelectionState state, bool instant)
        {
            Color targetColor = GetColorForState(state);

            if (!_targetGraphic)
                return;

            if (_currentTween != null)
                StopCoroutine(_currentTween);

            if (instant || _colors.fadeDuration <= 0f)
            {
                _targetGraphic.color = targetColor;
                return;
            }

            _currentTween = StartCoroutine(TweenColorRoutine(_targetGraphic, targetColor, _colors.fadeDuration));
        }
        
        private Color GetColorForState(SelectionState state)
        {
            Color swapColor = state switch
            {
                SelectionState.Normal => _colors.normalColor,
                SelectionState.Highlighted => _colors.highlightedColor,
                SelectionState.Pressed => _colors.pressedColor,
                SelectionState.Selected => _colors.selectedColor,
                SelectionState.Disabled => _colors.disabledColor,
                _ => Color.black
            };
            
            return swapColor * _colors.colorMultiplier;
        }
        
        private IEnumerator TweenColorRoutine(Graphic graphic, Color target, float duration)
        {
            Color start = graphic.color;
            float time = 0f;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(time / duration);
                float easedT = _easing.Evaluate(t);
                graphic.color = Color.Lerp(start, target, easedT);
                yield return null;
            }

            graphic.color = target;
            _currentTween = null;
        }
    }
}
