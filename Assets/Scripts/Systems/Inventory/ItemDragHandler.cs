using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 ogPos = Vector3.zero;

    public void OnDrag(PointerEventData eventData)
    {
        ogPos = transform.position;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //do if not on top of area0
        transform.position = ogPos;
    }
}
