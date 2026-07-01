using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace Rehawk.UIFramework.UIExtensions
{
    public class SelectableSpriteSwapHandler : SelectableStateHandlerBase
    {
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private SpriteState _spriteState;
        
        private Image Image
        {
            get => _targetGraphic as Image;
            set => _targetGraphic = value;
        }
        
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (isActiveAndEnabled)
            {
                DoSpriteSwap(null);
            }
            
            base.OnValidate();
        }
#endif
        
        protected override void HandleStateTransition(SelectionState state, bool instant)
        {
            Sprite newSprite = GetSpriteForState(state);
            DoSpriteSwap(newSprite);
        }
        
        private Sprite GetSpriteForState(SelectionState state)
        {
            return state switch
            {
                SelectionState.Normal => null,
                SelectionState.Highlighted => _spriteState.highlightedSprite,
                SelectionState.Pressed => _spriteState.pressedSprite,
                SelectionState.Selected => _spriteState.selectedSprite,
                SelectionState.Disabled => _spriteState.disabledSprite,
                _ => null
            };
        }
        
        private void DoSpriteSwap(Sprite newSprite)
        {
            if (!Image)
                return;

            Image.overrideSprite = newSprite;
        }
    }
}
