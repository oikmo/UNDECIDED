using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    Dictionary<invSlot, GameObject> itemsDisplayed = new Dictionary<invSlot, GameObject>();

    void Start()
    {
        CreateDisplay();
    }

    void Update()
    {
        if(inventory.Container.Count != transform.childCount-1)
        {
            ClearDisplay();
        }
        

        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponentInChildren<TMP_Text>().text = inventory.Container[i].amount.ToString("n0");
        }
    }

    public void ClearDisplay()
    {
        itemsDisplayed.Clear();
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
            
        {
            //ClearDisplay();
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TMP_Text>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponentInChildren<TMP_Text>().text = inventory.Container[i].amount.ToString("n0");
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }
    }
}
