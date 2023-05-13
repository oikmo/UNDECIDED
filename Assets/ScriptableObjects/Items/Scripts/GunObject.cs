using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun Object", menuName = "Inventory/Items/Gun")]
[System.Serializable]
public class GunObject : ItemObject
{
    public GunData data;
    public GunType gunType;
    public void Awake()
    {
        type = ItemType.Gun;
    }
}

