using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Gun", menuName="Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;
    public enum GunType 
    {
        Pistol,
        Rifle
    }
    public GunType type;
    public Sprite gun;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Shooting")]
    public int currentAmmo;
    public int magSize;
    public float fireRate;
    public float reloadTime;
    [HideInInspector]
    public bool reloading;
}
