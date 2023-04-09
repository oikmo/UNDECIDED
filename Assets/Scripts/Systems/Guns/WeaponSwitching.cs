using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using TMPro;
using System;

public class WeaponSwitching : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;
    [SerializeField] private NamedSprite[] pictures;

    private Guid Latest;
    public CanvasGroup gunHUD;

    [Header("Keys")]
    [SerializeField] private KeyCode[] keys;

    [Header("Settings")]
    [SerializeField] private float switchTime;

    public int selectedWeapon;
    int lastWeaponList;
    private float timeSinceLastSwitch;
    bool isEquipped = false;

    void Start()
    {
        InventoryObject store = GameHandler.Instance.playerInventory;
        GameHandler.Instance.playerInventory = null;
        GameHandler.Instance.playerInventory = store;
        SetWeapons();
        Select(selectedWeapon);
        timeSinceLastSwitch = 0f;
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        //print(selectedWeapon);

        if(GameHandler.Instance != null)
        {
            if(GameHandler.Instance.equipping)
            {
                StartCoroutine(Debounced(0.05f));
            }

            if(GameHandler.Instance.playerInventory.Container.Count == 0 && transform.childCount != 0)
            {
                foreach (Transform child in transform) 
                {
                    Destroy(child.gameObject);
                }

                keys = null;
                weapons = null;
            }


            if (weapons != null)
            {   
                foreach(Transform child in transform)
                {
                    if(!GameHandler.Instance.playerInventory.Container.Exists(x => x.item.itemName.ToLower() == child.name.ToLower()))
                    {
                        Destroy(child.gameObject);
                    }
            }   

            try
            {
                if (weapons[selectedWeapon].gameObject.activeSelf && isEquipped)
                {
                    gunHUD.alpha = 1f;
                }
                else
                {
                    gunHUD.alpha = 0f;
                }

                weapons[selectedWeapon].Find("Gun").gameObject.SetActive(isEquipped);

                for (int i = 0; i < keys.Length; i++)
                {
                    if (timeSinceLastSwitch >= switchTime)
                    {
                        if (GameHandler.Instance.curDevice == "keyboard&mouse")
                        {
                            if (Input.GetKeyDown(keys[i]))
                            {
                                if (GameHandler.Instance.playerInventory.Container.Any(p => p.ID == GameHandler.Instance.gunSlot[i].ID && p.item == GameHandler.Instance.gunSlot[i].item && p.amount == GameHandler.Instance.gunSlot[i].amount))
                                {
                                    selectedWeapon = i;
                                }

                            }

                        }

                        if (GameHandler.Instance.curDevice == "gamepad")
                        {
                            if (GameHandler.Instance.g_left)
                            {
                                if (GameHandler.Instance.playerInventory.Container.Any(p => p.ID == GameHandler.Instance.gunSlot[i].ID && p.item == GameHandler.Instance.gunSlot[i].item && p.amount == GameHandler.Instance.gunSlot[i].amount))
                                {
                                    if (selectedWeapon != 0)
                                    {
                                        selectedWeapon -= 1;
                                    }
                                    else
                                    {
                                        selectedWeapon = weapons.Length - 1;
                                    }

                                    Select(selectedWeapon);
                                }

                            }

                            if (GameHandler.Instance.g_right)
                            {
                                if (GameHandler.Instance.playerInventory.Container.Any(p => p.ID == GameHandler.Instance.gunSlot[i].ID && p.item == GameHandler.Instance.gunSlot[i].item && p.amount == GameHandler.Instance.gunSlot[i].amount))
                                {
                                    if (selectedWeapon != weapons.Length - 1)
                                    {
                                        selectedWeapon += 1;
                                    }
                                    else
                                    {
                                        selectedWeapon = 0;
                                    }

                                    Select(selectedWeapon);
                                }
                            }
                        }

                    }
                    if (GameHandler.Instance.playerInventory.Container.Count == 0)
                    {
                        weapons[i].Find("Gun").gameObject.SetActive(false);
                    }
                    if (previousSelectedWeapon != selectedWeapon) Select(selectedWeapon);

                    timeSinceLastSwitch += Time.deltaTime;
                }

            }
            catch (IndexOutOfRangeException) { }
            catch (NullReferenceException) { }
            

            
            } else
            {
                if(gunHUD.GetComponent<CanvasGroup>() != null) 
                {
                    gunHUD.alpha = 0f;
                }
            
            }
            if(lastWeaponList != transform.childCount)
            {
                SetWeapons();
            }
        }
        
    }

    private void Select(int weaponIndex)
    {
        if(weapons != null)
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                try
                {
                    weapons[i].Find("Gun").gameObject.SetActive(i == weaponIndex && isEquipped);
                }
                catch (NullReferenceException) { }
            }

            timeSinceLastSwitch = 0f;

            OnWeaponSelected();
        }
    }

    private void SetWeapons()
    {
        weapons = new Transform[transform.childCount];

        lastWeaponList = transform.childCount;

        for (int i = 0; i < transform.childCount; i++)
        {
            weapons[i] = transform.GetChild(i);
        }

        if(transform.childCount == 0)
        {
            weapons = null;
        }

        if (keys == null) keys = new KeyCode[weapons.Length];

        Select(selectedWeapon);
    }

    private void OnWeaponSelected()
    {
        print("selected new weapon...");
    }

    private IEnumerator Debounced(float delay)
    {
        // generate a new id and set it as the latest one 
        var guid = Guid.NewGuid();
        Latest = guid;

        // set the denounce duration here
        yield return new WaitForSeconds(delay);

        // check if this call is still the latest one
        if (Latest == guid)
        {
            isEquipped = !isEquipped;
        }
    }
}
[Serializable]
public struct NamedSprite {
     public string name;
     public Sprite image;
 }
