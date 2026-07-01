using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    public class ToggleValueColorSwapHandler : ToggleValueHandlerBase
    {
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private Color _offColor = Color.white;
        [SerializeField] private Color _onColor = Color.white;
        [SerializeField] private float _fadeDuration = 0.1f;
        [SerializeField] private float _colorMultiplier = 1f;
        [SerializeField] private AnimationCurve _easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Coroutine _currentTween;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            _fadeDuration = Mathf.Max(_fadeDuration, 0.0f);
            _colorMultiplier = Mathf.Max(_colorMultiplier, 0.0f);

            base.OnValidate();
        }
#endif

        protected override void HandleToggleValueChanged(bool isOn, bool instant)
        {
            if (_targetGraphic == null)
                return;

            Color targetColor = GetColorForValue(isOn);

            if (_currentTween != null)
            {
                StopCoroutine(_currentTween);
            }

            if (instant || _fadeDuration <= 0f)
            {
                _targetGraphic.color = targetColor;
                return;
            }

            _currentTween = StartCoroutine(TweenColorRoutine(_targetGraphic, targetColor, _fadeDuration));
        }

        private Color GetColorForValue(bool isOn)
        {
            return (isOn ? _onColor : _offColor) * _colorMultiplier;
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
