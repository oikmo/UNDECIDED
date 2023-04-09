using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Energy Object", menuName = "Inventory/Items/Energy")]
[System.Serializable]
public class EnergyObject : ItemObject
{
    public int restoreHealthValue;
    public void Awake()
    {
        type = ItemType.Energy;
    }
}
