using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Rehawk.UIFramework
{
    public class UIPanel : UIPanelBase
    {
        [SerializeField] private InitialVisibility visibility = InitialVisibility.None;
        [SerializeField] private ParentConstraint parentConstraint = ParentConstraint.None;
        
        [Space]
        
        [SubclassSelector]
        [SerializeReference] private VisibilityStrategyBase visibilityStrategy;
        
        private bool isInitialized;
        private bool wasPreviousVisible;
        
        private UIPanel parentUIPanel;

        public override event Action<UIPanel> BecameVisible;
        public override event Action<UIPanel> BecameInvisible;
        
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
                wasPreviousVisible = IsVisible;
                
                if (visibilityStrategy != null)
                {
                    visibilityStrategy.SetVisible(value, HandleVisibilityChange);
                }
                else
                {
                    gameObject.SetActive(value);
                    HandleVisibilityChange();
                }
            }
        }

        public override UIPanel Parent
        {
            get { return parentUIPanel; }
        }

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
                SetVisible(true);
            }
            
            if (visibility != InitialVisibility.None)
            {
                StartCoroutine(SetInitialVisibilityDelayed());
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

        public override void SetVisible(bool visible)
        {
            IsVisible = visible;
        }

        [ContextMenu("Toggle")]
        public override void ToggleVisible()
        {
            SetVisible(!IsVisible);
        }
        
        private void HandleVisibilityChange()
        {
            if (!isInitialized || wasPreviousVisible == IsVisible)
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
            
            // Do it one frame after Start to enable child controls Start too. 
            SetVisible(visibility == InitialVisibility.Visible);
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