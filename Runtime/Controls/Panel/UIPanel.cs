using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Rehawk.UIFramework
{
    /// <summary>
    /// Extends the functionality provided by UIPanelBase, allowing visibility
    /// management, parent panel relationships, and hierarchical visibility control by an inspector assignable <see cref="VisibilityStrategyBase"/>.
    /// </summary>
    public class UIPanel : UIPanelBase
    {
        [Tooltip("Determines the initial visibility state of the panel. None: Uses current state, Visible: Shows panel on start, Hidden: Hides panel on start")]
        [SerializeField] private InitialVisibility visibility = InitialVisibility.None;
        [Tooltip("Determines whether the panel should be visible when its parent panel becomes visible. None: Uses current state, HideWith: Hides panel when parent becomes invisible, " +
                 "ShowWith: Shows panel when parent becomes visible, HideAndShowWith: Hides panel when parent becomes invisible, and shows panel when parent becomes visible again")]
        [SerializeField] private ParentConstraint parentConstraint = ParentConstraint.None;
        
        [Space]
#if !ODIN_INSPECTOR   
        [SubclassSelector]
#endif
        [Tooltip("Defines the strategy used to control how the panel's visibility changes are handled and animated. If not set, the panel will use default GameObject activation")]
        [SerializeReference] private VisibilityStrategyBase visibilityStrategy;
        
        private bool isEnabled;
        private bool wasPreviousVisible;
        private bool isVisibilityInitializing;
        private bool wasVisibilitySetDuringInitialization;
        
        private UIPanel parentUIPanel;

        public override event Action<UIPanel> BecameVisible;
        public override event Action<UIPanel> BecameInvisible;
        
        public override bool Enabled
        {
            get => isEnabled;
            set => SetField(ref isEnabled, value);
        }

        public override bool IsVisible
        {
            get
            {
                if (visibilityStrategy != null)
                {
                    return visibilityStrategy.IsVisible;
                }
                
                return gameObject.activeSelf;
            }
            set
            {
                SetVisible(value);
            }
        }

        public override UIPanel Parent => parentUIPanel;

        protected override void Awake()
        {
            base.Awake();

            parentUIPanel = GetComponentsInParent<UIPanel>()
                .FirstOrDefault(p => p != this);

            if (parentUIPanel)
            {
                parentUIPanel.BecameVisible += OnParentUIPanelBecameVisible;    
                parentUIPanel.BecameInvisible += OnParentUIPanelBecameInvisible;    
            }

            if (visibility != InitialVisibility.None)
            {
                SetVisible(true, true);
            }

            isVisibilityInitializing = true;
            
            if (visibility != InitialVisibility.None)
            {
                StartCoroutine(SetInitialVisibilityDelayed());
            }
            else
            {
                isVisibilityInitializing = false;
            }
		}

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (parentUIPanel)
            {
                parentUIPanel.BecameVisible -= OnParentUIPanelBecameVisible;    
                parentUIPanel.BecameInvisible -= OnParentUIPanelBecameInvisible;    
            }
        }

        public override void SetVisible(bool visible, bool instant = false)
        {
            if (isVisibilityInitializing)
            {
                wasVisibilitySetDuringInitialization = true;
            }
            
            wasPreviousVisible = IsVisible;
            
            if (visibilityStrategy != null)
            {
                visibilityStrategy.SetVisible(this, visible, instant, HandleVisibilityChange);
            }
            else
            {
                gameObject.SetActive(visible);
                HandleVisibilityChange();
            }
        }

        [ContextMenu("Toggle")]
        public override void ToggleVisible()
        {
            SetVisible(!IsVisible);
        }
        
        private void HandleVisibilityChange()
        {
            if (wasPreviousVisible == IsVisible)
                return;
            
            if (IsVisible)
            {
                BecameVisible?.Invoke(this);
            }
            else
            {
                BecameInvisible?.Invoke(this);
            }
        }

        private IEnumerator SetInitialVisibilityDelayed()
        {
            yield return null;
            
            isVisibilityInitializing = false;
            
            // Do it one frame after Start to enable child controls Start too.
            if (!wasVisibilitySetDuringInitialization)
            {
                SetVisible(visibility == InitialVisibility.Visible, true);
            }
        }
        
        private void OnParentUIPanelBecameVisible(UIPanel panel)
        {
            if (parentConstraint == ParentConstraint.ShowWith || parentConstraint == ParentConstraint.HideAndShowWith)
            {
                SetVisible(true);
            }
        }

        private void OnParentUIPanelBecameInvisible(UIPanel panel)
        {
            if (parentConstraint == ParentConstraint.HideWith || parentConstraint == ParentConstraint.HideAndShowWith)
            {
                SetVisible(false);
            }
        }

        public enum InitialVisibility
        {
            None,
            Visible,
            Hidden
        }

        public enum ParentConstraint
        {
            None,
            HideWith,
            ShowWith,
            HideAndShowWith,
        }
    }
}
