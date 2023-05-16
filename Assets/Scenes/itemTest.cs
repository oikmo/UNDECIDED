using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTest : MonoBehaviour {
    public InventoryManager inventoryManager;
    public ItemObject[] itemsToPickup;

    public void PickUpTime(int id) {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (!result) {
            print("INV FULL!!!");
        }
    }

    public void GetSelectedItem() {
        ItemObject recievedItem = inventoryManager.GetSelectedItem();
        if (recievedItem != null) {
            print("Recieved Item! : " + recievedItem.itemName);
        } else {
            print("No item recieved!");
        }
    }

    public void UseSelectedItem(int count) {
        ItemObject recievedItem = inventoryManager.UseSelectedItem(count);
        if (recievedItem != null) {
            print("Recieved Item! : " + recievedItem.itemName);
        } else {
            print("No item recieved!");
        }
    }

}
