using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Object", menuName = "Inventory/Items/Ammo")]
[System.Serializable]
public class AmmoObject : ItemObject
{
    public enum AmmoType
    {
        Pistol,
        Rifle
    }

    public AmmoType ammoType;

    public void Awake()
    {
        type = ItemType.Ammo;
    }
}

