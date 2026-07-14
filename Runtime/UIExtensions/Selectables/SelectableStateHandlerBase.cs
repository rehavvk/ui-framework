using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    /// <summary>
    /// Base class for custom UI selectable state handling.
    /// 
    /// This component mirrors Unity's internal <see cref="Selectable"/> state logic
    /// (Normal, Highlighted, Pressed, Selected, Disabled) but exposes state transitions
    /// in a controlled and extensible way for custom visuals or behavior.
    /// 
    /// It listens to pointer and selection events, evaluates the current interaction
    /// state each frame, and invokes <see cref="HandleStateTransition"/> only when the
    /// effective selection state changes.
    ///
    /// State transitions are evaluated continuously to ensure consistency even when
    /// external systems modify selectable interactability or selection.
    /// 
    /// Intended to be subclassed to implement custom transitions (animations, colors,
    /// audio, etc.) without relying on Unity's built-in Selectable transition system.
    /// </summary>
    public abstract class SelectableStateHandlerBase : UIBehaviour,
                                                   IPointerDownHandler, IPointerUpHandler,
                                                   IPointerEnterHandler, IPointerExitHandler,
                                                   ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Selectable _selectable;
        
        private bool _enableCalled = false;

        private bool _isPointerInside;
        private bool _isPointerDown;
        private bool _hasSelection;

        protected SelectionState PreviousSelectionState { get; private set; }

        protected SelectionState CurrentSelectionState
        {
            get
            {
                if (_selectable && !_selectable.IsInteractable())
                {
                    return SelectionState.Disabled;
                }

                if (_isPointerDown)
                {
                    return SelectionState.Pressed;
                }

                if (_hasSelection)
                {
                    return SelectionState.Selected;
                }

                if (_isPointerInside)
                {
                    return SelectionState.Highlighted;
                }

                return SelectionState.Normal;
            }
        }
        
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            _selectable ??= GetComponent<Selectable>();
        }
#endif
        
        protected override void Awake()
        {
            base.Awake();
            
            _selectable ??= GetComponent<Selectable>();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            _selectable ??= GetComponent<Selectable>();

            // OnValidate can be called before OnEnable, this makes it unsafe to access other components
            // since they might not have been initialized yet.
            if (isActiveAndEnabled)
            {
                if ((_selectable == null || !_selectable.IsInteractable()) && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == gameObject)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }

                DoStateTransition(CurrentSelectionState, true);
            }
        }
#endif
        
        protected override void OnEnable()
        {
            if (_enableCalled)
                return;
            
            base.OnEnable();

            DoInitialTransition();
            
            _enableCalled = true;
        }

        protected override void Start()
        {
            base.Start();

            DoInitialTransition();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _isPointerInside = false;
            _isPointerDown = false;
            _hasSelection = false;
            _enableCalled = false;
        }

        private void Update()
        {
            DoStateTransition(CurrentSelectionState, false);
        }

        private void DoInitialTransition()
        {
            if (!isActiveAndEnabled)
                return;

            if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                _hasSelection = true;
            }
            
            DoStateTransition(CurrentSelectionState, true);
        }

        private void DoStateTransition(SelectionState state, bool instant)
        {
            if (!isActiveAndEnabled)
                return;

            if (PreviousSelectionState == state)
                return;

            PreviousSelectionState = state;

            HandleStateTransition(state, instant);
        }

        protected abstract void HandleStateTransition(SelectionState state, bool instant);

#if UNITY_EDITOR
        public void PreviewStateTransition(SelectionState state)
        {
            if (!isActiveAndEnabled)
                return;

            PreviousSelectionState = state;
            HandlePreviewStateTransition(state);
        }

        public void RestoreCurrentStateTransition()
        {
            if (!isActiveAndEnabled)
                return;

            PreviousSelectionState = CurrentSelectionState;
            HandleRestoreCurrentStateTransition();
        }

        protected virtual void HandlePreviewStateTransition(SelectionState state)
        {
            HandleStateTransition(state, true);
        }

        protected virtual void HandleRestoreCurrentStateTransition()
        {
            HandleStateTransition(CurrentSelectionState, true);
        }
#endif
        
        private void EvaluateAndTransitionToSelectionState()
        {
            if (!IsActive() || (_selectable != null && !_selectable.IsInteractable()))
                return;

            DoStateTransition(CurrentSelectionState, false);
        }
        
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _isPointerDown = true;
            EvaluateAndTransitionToSelectionState();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _isPointerDown = false;
            EvaluateAndTransitionToSelectionState();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            _isPointerInside = true;
            EvaluateAndTransitionToSelectionState();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            _isPointerInside = false;
            EvaluateAndTransitionToSelectionState();
        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            _hasSelection = true;
            EvaluateAndTransitionToSelectionState();
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            _hasSelection = false;
            EvaluateAndTransitionToSelectionState();
        }
        
        public enum SelectionState
        {
            Normal,
            Highlighted,
            Pressed,
            Selected,
            Disabled,
        }
    }
}
