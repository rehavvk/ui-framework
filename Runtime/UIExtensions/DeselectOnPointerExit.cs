using UnityEngine;
using UnityEngine.EventSystems;

namespace Rehawk.UIFramework.UIExtensions
{
    /// <summary>
    /// Unity doesn't deselect selectables when they get disabled. By listening to the OnPointerExit event which is also called when the object gets unreachable for the pointer the issue can be fixed.
    /// Doesn't work for touch and requires a default selection handling for joysticks.
    /// </summary>
    public class DeselectOnPointerExit : MonoBehaviour, IPointerExitHandler
    {
        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}