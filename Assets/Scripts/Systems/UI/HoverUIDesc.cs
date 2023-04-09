using UnityEngine;
using UnityEngine.EventSystems;

public class HoverUIDesc : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ItemObject item;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<HoverUIDesc>() != null)
            {
                HoverUI.tempItem = item;
                HoverUI.isFollow = true;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverUI.isFollow = false;
        HoverUI.tempItem = null;
    }
}