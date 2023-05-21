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

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f) && !GameHandler.Instance.paused && gunModel.activeSelf;

    private void Shoot() {
        //print("ran! " + (gunData.curAmmoSize > 0 && gunModel.activeSelf) + " " + gunData.curAmmoSize + " " + gunModel.activeSelf);
        if (gunData.curAmmoSize > 0) {
            if (CanShoot()) {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.range)) {
                    IDamagable damageable = hitInfo.transform.GetComponent<IDamagable>();
                    damageable?.Damage(gunData.damage);
                }

                gunData.curAmmoSize--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        } else {
            StartReload();
        }
    }

    public void StartReload() {
        if (!gunData.reloading && gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload() {
        gunData.reloading = true;
        PlaySound(2, gunData.onReload);
        yield return new WaitForSeconds(gunData.reloadTime);
        

        if (gunData.curAmmoSize == 0) { requiredAmmo = gunData.ammoSize; } else { requiredAmmo = Math.Abs(gunData.curAmmoSize - gunData.ammoSize); }

        if(gunData.item.gunType == GunType.Pistol && GameHandler.Instance.ammoValues[0] != 0) {
            //print((GameHandler.Instance.ammoValues[0] < requiredAmmo) + " " + GameHandler.Instance.ammoValues[0] + " " + requiredAmmo);
            if (GameHandler.Instance.ammoValues[0] == requiredAmmo) {
                GameHandler.Instance.ammoValues[0] = 0;
                gunData.curAmmoSize += requiredAmmo;
            } else if(GameHandler.Instance.ammoValues[0] < requiredAmmo) {
                gunData.curAmmoSize = GameHandler.Instance.ammoValues[0];
                GameHandler.Instance.ammoValues[0] = 0;
                
            } else if(!(GameHandler.Instance.ammoValues[0] - requiredAmmo <= 0)) {
                GameHandler.Instance.ammoValues[0] -= requiredAmmo;
                gunData.curAmmoSize += requiredAmmo;
            }

            
        }
        GameHandler.Instance.RefreshValues();
        gunData.reloading = false;
    }

    private void PlaySound(int i, AudioClip clip) {
        speaker[i].Stop();
        speaker[i].clip = clip;
        speaker[i].Play();
    }

    public void Update() {
        timeSinceLastShot += Time.deltaTime;
    }

    private void OnGunShot() {
        PlaySound(3, gunData.onShot);
    }

}