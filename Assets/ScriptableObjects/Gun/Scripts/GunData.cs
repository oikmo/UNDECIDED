using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Gun", menuName="Weapon/Gun")]
public class GunData : ScriptableObject
{

    [Header("Info")]
    public GunObject item;
    public int damage;

    [Header("Mag Size")]
    public int ammoSize;
    public int curAmmoSize;

    [Header("Timing")]
    public float fireRate;
    public float reloadTime;
    public float slowDownMutliplier;
    public float range;

    [Header("Sound")]
    public AudioClip onEquip;
    public AudioClip onDequip;
    public AudioClip onShot;
    public AudioClip onReload;

    public bool reloading;
}
