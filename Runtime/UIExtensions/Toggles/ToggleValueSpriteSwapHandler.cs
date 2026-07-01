using UnityEngine;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    public class ToggleValueSpriteSwapHandler : ToggleValueHandlerBase
    {
        [SerializeField] private Image _targetImage;
        [SerializeField] private Sprite _offSprite;
        [SerializeField] private Sprite _onSprite;

        protected override void HandleToggleValueChanged(bool isOn, bool instant)
        {
            if (_targetImage == null)
                return;

            _targetImage.overrideSprite = isOn ? _onSprite : _offSprite;
        }
    }
}
