using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    

    [Header("UI")]
    [HideInInspector] public Image image;
    public TMP_Text textCount;

    [Header("Components")]
    [HideInInspector] public ItemObject item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(ItemObject newItem)
    {
        item = newItem;
        image.sprite = newItem.icon;
        RefreshCount();
    }

    public void RefreshCount()
    {
        textCount.text = count.ToString();
        textCount.gameObject.SetActive(count > 1);
        textCount.gameObject.transform.parent.gameObject.SetActive(count > 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        if (inventoryManager.isOn()) { print("!"); return; }
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        InventoryManager inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        if (inventoryManager.isOn()) { print("!"); return; }
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventoryManager inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
        if (inventoryManager.isOn()) { print("!"); return; }
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        transform.position = parentAfterDrag.position;
    }
}
