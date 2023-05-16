using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTest : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemObject[] itemsToPickup;

    public void PickUpTime(int id)
    {
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if (!result)
        {
            print("INV FULL!!!");
        }
    }

}
