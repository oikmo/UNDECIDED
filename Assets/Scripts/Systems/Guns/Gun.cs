using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Gun : MonoBehaviour
{
    [Header("References")]
    public GunData gunData;
    private Transform cam;
    [SerializeField] private GameObject gunModel;
    private TMP_Text ammoSize;
    private TMP_Text g_name;
    private Image image;

    int requiredAmmo = 0;
    float timeSinceLastShot;

    private void Start()
    {
        GameObject tempObject1 = GameObject.Find("Gun Sway");
        GameObject tempObject2 = GameObject.Find("CameraHolder");
        

        if (tempObject1 != null && tempObject2 != null)
        {
            cam = tempObject2.GetComponent<Transform>();
            
            if(tempObject1.transform.Find("Gun_Ammo") != null)
            {
                ammoSize = tempObject1.transform.Find("Gun_Ammo").GetComponent<TMP_Text>();
                ammoSize.SetText("");
            }

            if(tempObject1.transform.Find("Gun_Name") != null)
            {
                g_name = tempObject1.transform.Find("Gun_Name").GetComponent<TMP_Text>();
                g_name.SetText("");
            }

            if(tempObject1.transform.Find("Gun_Image") != null)
            {
                image = tempObject1.transform.Find("Gun_Image").GetComponent<Image>();
            }
            
        }

        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;
    }

    private void OnDisable() => gunData.reloading = false;

    public void StartReload()
    {
        if (!gunData.reloading && this.gameObject.activeSelf)
            StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        gunData.reloading = true;

        yield return new WaitForSeconds(gunData.reloadTime);

        if (gunData.currentAmmo != 0)
        {
            requiredAmmo = gunData.magSize - gunData.currentAmmo;
        }
        else
        {
            requiredAmmo = gunData.magSize;
        }

        print(requiredAmmo);

        for(int i = 0; i<GameHandler.Instance.playerInventory.Container.Count - 1; i++)
        {
            if(GameHandler.Instance.playerInventory.Container.Any(p => p.ID == GameHandler.Instance.ammoSlot[i].ID && p.item == GameHandler.Instance.ammoSlot[i].item))
            {
                if (gunData.type == GunData.GunType.Pistol)
                {
                    if(GameHandler.Instance.playerInventory.Container[i].GetType() == typeof(AmmoObject))
                    {
                        AmmoObject ammo = GetType(GameHandler.Instance.playerInventory.Container[i].item);
                    }
                }
            }
            
        }

        gunData.currentAmmo = gunData.magSize;

        gunData.reloading = false;
    }

    AmmoObject GetType(ItemObject item)
    {
        object temp = item;
        AmmoObject ammo = (AmmoObject)temp;
        return ammo;
    }

    private bool CanShoot() => !gunData.reloading && timeSinceLastShot > 1f / (gunData.fireRate / 60f);

    private void Shoot()
    {

        if (gunData.currentAmmo > 0 && !GameHandler.Instance.paused && gunModel.activeSelf)
        {
            if (CanShoot())
            {
                if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hitInfo, gunData.maxDistance))
                {
                    IDamagable damageable = hitInfo.transform.GetComponent<IDamagable>();
                    damageable?.Damage(gunData.damage);
                }

                gunData.currentAmmo--;
                timeSinceLastShot = 0;
                OnGunShot();
            }
        }
    }

    private void Update()
    {
        //print(gunData.name + " " + gunModel.activeSelf);
        if(gunModel.activeSelf)
        {
            if(g_name.text != gunData.name) 
            {
                g_name.SetText(gunData.name);
            }
            
            if(ammoSize.text != gunData.currentAmmo.ToString() + " / " + gunData.magSize.ToString()) 
            {
                ammoSize.SetText(gunData.currentAmmo.ToString() + " / " + gunData.magSize.ToString());
            }
            
            if(image != null) 
            {
                if(image.sprite != gunData.gun)
                {
                    image.sprite = gunData.gun;
                }
            }
            

        }

        timeSinceLastShot += Time.deltaTime;
        if(GameHandler.Instance != null)
        {
            if(GameHandler.Instance.playerInput != null)
            {
                if (GameHandler.Instance.firing && !gunData.reloading && gunData.currentAmmo == 0 && !GameHandler.Instance.paused)
                {
                    StartReload();
                }
            }
        }
        
        
    }

    private void OnGunShot() 
    {
        try
        {
            StartCoroutine(doCoundDown(1));
        } catch (NullReferenceException) { }
        
    }

    public IEnumerator doCoundDown(int seconds)
    {
        try
        {
            GameHandler.Instance.SetGlitch(0.082f, 0.1f, 0.1f, 0.2f, 0.1f);
        }
        catch (NullReferenceException) { }
        
        for (int time = seconds; time > 0; time--)
        {
             yield return new WaitForSeconds(.125f);
        }
        try
        {
            GameHandler.Instance.SetGlitch(0, 0, 0, 0, 0);
        }
        catch (NullReferenceException) { }



    }
}