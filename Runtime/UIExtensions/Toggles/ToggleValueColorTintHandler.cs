using UnityEngine;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    public class ToggleValueColorTintHandler : ToggleValueHandlerBase
    {
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private Color _offColor = Color.white;
        [SerializeField] private Color _onColor = Color.white;
        [SerializeField] private float _fadeDuration = 0.1f;
        [SerializeField] private float _colorMultiplier = 1f;

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
            _targetGraphic.CrossFadeColor(targetColor, instant ? 0f : _fadeDuration, true, true);
        }

        private Color GetColorForValue(bool isOn)
        {
            return (isOn ? _onColor : _offColor) * _colorMultiplier;
        }
    }
}
