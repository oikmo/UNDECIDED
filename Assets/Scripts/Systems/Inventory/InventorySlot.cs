using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    Image image;
    public Color selectedColor, notSelectedColor;
    public float selectedSize, notSelectedSize;

    public bool selected = false;

    private void Awake()
    {
        image = GetComponent<Image>();
        Deselect();
    }
    public void Select()
    {
        image.pixelsPerUnitMultiplier = selectedSize;
        image.color = selectedColor;
        selected = true;
    }

    public void Deselect()
    {
        //print("deselect!");
        image.pixelsPerUnitMultiplier = notSelectedSize;
        image.color = notSelectedColor;
        selected = false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0) {
            InventoryItem invItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            invItem.parentAfterDrag = transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null) { return; }
        if (!GameObject.Find("InventoryManager")) { return; }
        if (!GameObject.Find("InventoryManager").GetComponent<InventoryManager>().isOn()) { return; }
        itemInSlot.outline.enabled = true;
        HoverUI.tempItem = itemInSlot.item;
        HoverUI.isFollow = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        InventoryItem itemInSlot = GetComponentInChildren<InventoryItem>();
        if (itemInSlot == null) { return; }
        if (!GameObject.Find("InventoryManager")) { return; }
        if (!GameObject.Find("InventoryManager").GetComponent<InventoryManager>().isOn()) { return; }

        itemInSlot.outline.enabled = false;
        HoverUI.tempItem = null;
        HoverUI.isFollow = false;
    }
}
