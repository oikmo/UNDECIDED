using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Gun : MonoBehaviour
{
    public AudioSource[] speaker;
    public GunData gunData;
    private Transform cam;
    [SerializeField] private GameObject gunModel;
    int requiredAmmo = 0;
    float timeSinceLastShot;

    public void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void onDisable() => gunData.reloading = false;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot()
    {
        if (gunData.curAmmoSize > 0 && !GameHandler.Instance.paused && gunModel.activeSelf)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.range))
                {
                    IDamagable damageable = hitInfo.transform.GetComponent<IDamagable>();
                    damageable?.Damage(gunData.damage);
                }

                gunData.curAmmoSize--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    public void StartReload()
    {
        if (!gunData.reloading && gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        InventoryItem ammoItem = null;
        AmmoObject ammo = null;
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);

        for (int i = 0; i < GameHandler.Instance.playerInventory.inventorySlots.Length; i++)
        {
            //if (GameHandler.Instance.playerInventory.Container.Any(p => p.ID == GameHandler.Instance.refAmmoSlot[i].ID && p.item == GameHandler.Instance.refAmmoSlot[i].item))
            if (GameHandler.Instance.playerInventory.GetItemByIndex(i).GetType() == typeof(AmmoObject)) {
                ammoItem = GameHandler.Instance.playerInventory.GetInvItemByIndex(i);

                if (GetType(ammoItem.item).ammoType == gunData.item.gunType) {
                    ammo = GetType(ammoItem.item);
                }
            }

        }

        if(ammoItem != null)
        {
            if (gunData.curAmmoSize == 0) { requiredAmmo = gunData.ammoSize; }
            else { requiredAmmo = gunData.curAmmoSize - gunData.ammoSize; }

            if (!(ammoItem.count - requiredAmmo < 0))
            {
                PlaySound(2, gunData.onReload);
                ammoItem.count -= requiredAmmo;
                gunData.curAmmoSize += requiredAmmo;
            }
        }

        gunData.reloading = false;
    }

    private void PlaySound(int i, AudioClip clip)
    {
        speaker[i].Stop();
        speaker[i].clip = clip;
        speaker[i].Play();
    }

    AmmoObject GetType(ItemObject item)
    {
        object temp = item;
        AmmoObject ammo = (AmmoObject)temp;
        return ammo;
    }

    public void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShot()
    {
        PlaySound(3, gunData.onShot);
    }

}