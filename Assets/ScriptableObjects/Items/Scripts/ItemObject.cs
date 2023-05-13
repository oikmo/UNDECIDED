using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Ammo,
    Energy,
    Gun,
    Default
}
public enum GunType
{ 
    Pistol,
    Rifle
}

[System.Serializable]
public class ItemObject : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public ItemType type;
    [TextArea(15, 20)]
    public string description;
    public int maxStackSize;
    public float pickUpTime;
    public Sprite icon;
}
