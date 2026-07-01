using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    /// <summary>
    /// Base class for visual state that depends on a Toggle's value.
    /// </summary>
    public abstract class ToggleValueHandlerBase : UIBehaviour
    {
        [SerializeField] private Toggle _toggle;

        protected bool CurrentToggleValue => _toggle != null && _toggle.isOn;

        protected override void Reset()
        {
            base.Reset();

            _toggle ??= GetComponent<Toggle>();
        }

        protected override void Awake()
        {
            base.Awake();

            _toggle ??= GetComponent<Toggle>();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _toggle ??= GetComponent<Toggle>();

            if (isActiveAndEnabled && ShouldApplyToggleValueOnValidate())
            {
                ApplyToggleValue(true);
            }
        }
#endif

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_toggle == null)
                return;

            _toggle.onValueChanged.AddListener(HandleValueChanged);
            ApplyToggleValue(true);
        }

        protected override void OnDisable()
        {
            if (_toggle != null)
            {
                _toggle.onValueChanged.RemoveListener(HandleValueChanged);
            }

            base.OnDisable();
        }

        private void HandleValueChanged(bool isOn)
        {
            if (!isActiveAndEnabled)
                return;

            HandleToggleValueChanged(isOn, false);
        }

        private void ApplyToggleValue(bool instant)
        {
            if (!isActiveAndEnabled || _toggle == null)
                return;

            HandleToggleValueChanged(_toggle.isOn, instant);
        }

#if UNITY_EDITOR
        public bool EditorCurrentToggleValue => CurrentToggleValue;

        public void PreviewToggleValue()
        {
            if (!isActiveAndEnabled)
                return;

            HandlePreviewToggleValue();
        }

        public void RestoreToggleValue()
        {
            if (!isActiveAndEnabled)
                return;

            HandleRestoreToggleValue();
        }

        public void PreviewValueTransition(bool isOn)
        {
            if (!isActiveAndEnabled)
                return;

            HandlePreviewValueTransition(isOn);
        }

        protected virtual void HandlePreviewToggleValue()
        {
            ApplyToggleValue(true);
        }

        protected virtual void HandleRestoreToggleValue()
        {
            ApplyToggleValue(true);
        }

        protected virtual void HandlePreviewValueTransition(bool isOn)
        {
            HandleToggleValueChanged(isOn, true);
        }

        protected virtual bool ShouldApplyToggleValueOnValidate()
        {
            return true;
        }
#endif

        protected abstract void HandleToggleValueChanged(bool isOn, bool instant);
    }
}
