using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject mainInv;
    public GameObject toolBar;

    private float nextKey = -1f;

    public void Update()
    {
        if (Input.GetKey(KeyCode.I) && Time.time > nextKey)
        {
            nextKey = Time.time + 0.2f;
            mainInv.SetActive(!mainInv.activeSelf);
        }

        if(isOn() && toolBarPos().y != -177)
        {
            TransformExtentions.SetLocalY(toolBar.transform, -177);
        } 
        else if(!isOn() && toolBarPos().y != 234)
        {

            TransformExtentions.SetLocalY(toolBar.transform, 234);
        }

    }

    public bool AddItem(ItemObject item)
    {
        //check for anny slot that isnt max
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.item.isStackable)
            {
                if (itemInSlot.count < itemInSlot.item.maxStackSize)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    return true;
                }
                
                
            }
        }
        //check for empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    public void SpawnNewItem(ItemObject item, InventorySlot slot)
    { 
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem invItem = newItemGo.GetComponent<InventoryItem>();
        invItem.InitialiseItem(item);
    }
    public bool isOn() => mainInv.activeSelf;

    Vector2 toolBarPos()
    {
        return toolBar.transform.position;
    }
}
