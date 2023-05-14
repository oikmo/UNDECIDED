using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotTest : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] Image image;
    private ItemObject _item;
    public ItemObject item
    {
        get { return _item; }
        set
        {
            _item = value;

            if(_item == null)
            {
                image.enabled = false;
            } else
            {
                image.sprite = _item.icon;
                image.enabled = true;
            }
        }
    }

    private void OnValidate()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    Vector2 originalPosition;
    public void OnDrag(PointerEventData eventData)
    {
        print("drag!");
        image.transform.position = Input.mousePosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        print("begin!");
        originalPosition = image.transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("im tired");
        image.transform.position = originalPosition;
    }

    
}
