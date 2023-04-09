using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Item : MonoBehaviour
{
    public ItemObject item;
    public int amount;
    [SerializeField] GameObject model;
    public bool isBeingHeld;
    
    public void moveItem()
    {
        if(amount != 0)
        {
            GameHandler.Instance.playerInventory.AddItem(item, amount);
            Cicero.Instance.DisplayText("You have picked up '" + item.itemName + "' x" + amount);

            if (model != null && item.type == ItemType.Gun)
            {
                GameObject temp = Instantiate(model, GameObject.Find("WeaponHolder").transform);
                temp.name = temp.name.Replace("(Clone)", "");
            }
        }
        else
        {
            Cicero.Instance.DisplayText("You didn't pick up '" + item.itemName + "' because it's empty!");
        }

        Destroy(gameObject);
    }
}
