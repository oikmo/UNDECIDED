using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour {
    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject mainInv;
    public GameObject toolBar;

    int selectedSlot = -1;
    void ChangeSelectedSlot(int newValue) {
        if (selectedSlot >= 0) {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;

    }
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            inventorySlots[0].Deselect();
            inventorySlots[1].Deselect();
        }

        if (Input.inputString != null) {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number > 0 && number < 3) {
                if (Input.GetKeyDown(KeyCode.Alpha0)) {
                    //print("hiii!");
                    inventorySlots[0].Deselect();
                    inventorySlots[1].Deselect();
                    selectedSlot = -1;
                } else { ChangeSelectedSlot(number - 1); }
                
                
            }
        }

        if (isOn() && toolBarPos().y != -177) {
            TransformExtentions.SetLocalY(toolBar.transform, -177);
        } else if (!isOn() && toolBarPos().y != 194.3f) {
            TransformExtentions.SetLocalY(toolBar.transform, 194.3F);
        }

        if(!isOn()) {
            for (int i = 0; i < inventorySlots.Length; i++) {
                InventorySlot slot = inventorySlots[i];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null) {
                    itemInSlot.outline.enabled = false;
                }
            }
            if(HoverUI.tempItem != null) {
                HoverUI.tempItem = null;
                HoverUI.isFollow = false;
            }
        }
        

    }

    public bool AddItem(ItemObject item) {
        //check for anny slot that isnt max
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.item.isStackable) {
                if (itemInSlot.count < itemInSlot.item.maxStackSize) {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    return true;
                }


            }
        }
        //check for empty slot
        for (int i = 0; i < inventorySlots.Length; i++) {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
            if (itemInSlot == null) {
                SpawnNewItem(item, slot);
                return true;
            }
        }

        return false;
    }

    public void SpawnNewItem(ItemObject item, InventorySlot slot) {
        if (item == null) { print("GOOO"); return; }
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem invItem = newItemGo.GetComponent<InventoryItem>();
        invItem.InitialiseItem(item);
    }
    public bool isOn() => mainInv.activeSelf;

    Vector2 toolBarPos() {
        return toolBar.transform.position;
    }

    public ItemObject GetItemByIndex(int i) {
        return inventorySlots[i].GetComponentInChildren<InventoryItem>().item;
    }

    public InventoryItem GetInvItemByIndex(int i) {
        return inventorySlots[i].GetComponentInChildren<InventoryItem>();
    }

    public ItemObject GetSelectedItem() {
        InventoryItem itemInSlot = null;
        try {
            itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        } catch (IndexOutOfRangeException) {
            return null;
        }
        
        if (itemInSlot != null && !isNotSelected()) {
            ItemObject item = itemInSlot.item;
            return item;
        }

        return null;
    }

    public ItemObject UseSelectedItem(int count) {
        InventoryItem itemInSlot = inventorySlots[selectedSlot].GetComponentInChildren<InventoryItem>();
        if (itemInSlot != null) {
            ItemObject item = itemInSlot.item;
            itemInSlot.count--;
            if (itemInSlot.count <= 0) {
                Destroy(itemInSlot.gameObject);
            } else {
                itemInSlot.RefreshCount();
            }

            return item;
        }

        return null;
    }

    public bool isNotSelected() {
        return !inventorySlots[0].selected && !inventorySlots[1].selected;
    }

    public bool isOneSelected() {
        return inventorySlots[0].selected || inventorySlots[1].selected;
    }
}
 