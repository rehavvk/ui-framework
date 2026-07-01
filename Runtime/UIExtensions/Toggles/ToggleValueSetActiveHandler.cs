using UnityEngine;

namespace Rehawk.UIFramework.UIExtensions
{
    public class ToggleValueSetActiveHandler : ToggleValueHandlerBase
    {
        [SerializeField] private GameObject _onTargetObject;
        [SerializeField] private GameObject _offTargetObject;

        protected override void HandleToggleValueChanged(bool isOn, bool instant)
        {
            if (_onTargetObject != null)
            {
                _onTargetObject.SetActive(isOn);
            }

            if (_offTargetObject != null)
            {
                _offTargetObject.SetActive(!isOn);
            }
        }
    }
}
