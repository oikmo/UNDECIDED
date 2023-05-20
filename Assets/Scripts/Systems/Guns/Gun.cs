using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Gun : MonoBehaviour {
    public AudioSource[] speaker;
    public GunData gunData;
    [SerializeField] Transform cam;
    [SerializeField] private GameObject gunModel;
    int requiredAmmo = 0;
    float timeSinceLastShot;

    public void Start() {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void onDisable() => gunData.reloading = false;

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && !GameHandler.Instance.paused;

    private void Shoot() {
        //print("ran! " + (gunData.curAmmoSize > 0 && gunModel.activeSelf) + " " + gunData.curAmmoSize + " " + gunModel.activeSelf);
        if (gunData.curAmmoSize > 0 && gunModel.activeSelf) {
            print("ran2!");
            if (CanShoot()) {
                print("ran3!");
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.range)) {
                    IDamagable damageable = hitInfo.transform.GetComponent<IDamagable>();
                    damageable?.Damage(gunData.damage);
                }

                gunData.curAmmoSize--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    public void StartReload() {
        if (!gunData.reloading && gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        InventoryItem ammoItem = null;
        AmmoObject ammo = null;
        gunData.reloading = true;
        yield return new WaitForSeconds(gunData.reloadTime);

        print("run!");
        for (int thatI = 0; thatI < GameHandler.Instance.playerInventory.inventorySlots.Length - 2; thatI++) {
            int i = thatI + 2;
            //if (GameHandler.Instance.playerInventory.Container.Any(p => p.ID == GameHandler.Instance.refAmmoSlot[i].ID && p.item == GameHandler.Instance.refAmmoSlot[i].item))
            //print(GameHandler.Instance.playerInventory.GetItemByIndex(i));
            if (GameHandler.Instance.playerInventory.GetItemByIndex(i) != null) {
                if (GameHandler.Instance.playerInventory.GetItemByIndex(i).type == ItemType.Ammo) {
                    ammoItem = GameHandler.Instance.playerInventory.GetInvItemByIndex(i);

                    if (GetType(ammoItem.item).ammoType == gunData.item.gunType) {
                        ammo = GetType(ammoItem.item);
                    }
                }
            } else {
                print("didnt have any ammo?");
            }
        }

        if (ammoItem != null) {
            if (gunData.curAmmoSize == 0) { requiredAmmo = gunData.ammoSize; } else { requiredAmmo = gunData.curAmmoSize - gunData.ammoSize; }

            if (!(ammoItem.count - requiredAmmo < 0)) {
                PlaySound(2, gunData.onReload);
                ammoItem.count -= requiredAmmo;
                gunData.curAmmoSize += requiredAmmo;
            }
        }

        gunData.reloading = false;
    }

    private void PlaySound(int i, AudioClip clip) {
        speaker[i].Stop();
        speaker[i].clip = clip;
        speaker[i].Play();
    }

    AmmoObject GetType(ItemObject item) {
        object temp = item;
        AmmoObject ammo = (AmmoObject)temp;
        return ammo;
    }

    public void Update() {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShot() {
        print("hii");
        PlaySound(3, gunData.onShot);
    }

}