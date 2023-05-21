using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using TMPro;
using System;

public class WeaponSwitching : MonoBehaviour {
    [SerializeField] GameObject[] weapons;
    int currentlySelected = 0;
    GunData curGunData = null;
    public void Update() {
        if (GameHandler.Instance.playerInventory.GetSelectedItem()) {
            if (currentlySelected != -1) {
                if(curGunData != weapons[currentlySelected].transform.Find(weapons[currentlySelected].name).GetComponent<Gun>().gunData)
                    curGunData = weapons[currentlySelected].transform.Find(weapons[currentlySelected].name).GetComponent<Gun>().gunData;
            } else if(currentlySelected == -1 && curGunData != null) {
                curGunData = null;
            }

            if (curGunData != null) {
                if (GameHandler.Instance.gunText[0].text != curGunData.item.name) {
                    GameHandler.Instance.gunText[0].text = curGunData.item.name;
                }
                if (GameHandler.Instance.gunText[1].text != curGunData.curAmmoSize + "/" + curGunData.ammoSize) {
                    GameHandler.Instance.gunText[1].text = curGunData.curAmmoSize + "/" + curGunData.ammoSize;
                }
            }

            for (int i = 0; i < weapons.Length; i++) {
                switch (GameHandler.Instance.playerInventory.GetSelectedItem().itemName) {
                    case "Pistolinie":
                        SetWeapon(1);
                        break;
                    case "Pistol":
                        SetWeapon(0);
                        break;
                }
            }
        } else {
            //print("FALSE!");
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            if(currentlySelected != -1) {
                currentlySelected = -1;
            }

            if (GameHandler.Instance.gunText[0].text != "") {
                GameHandler.Instance.gunText[0].text = "";
            }
            if (GameHandler.Instance.gunText[1].text != "") {
                GameHandler.Instance.gunText[1].text = "";
            }
        }

    }

    void SetWeapon(int index) {
        weapons[index].SetActive(true);
        currentlySelected = index;
        for (int i = 0; i<weapons.Length; i++) {
            if(i==index) { return; }
            
            weapons[i].SetActive(false);
        }
    }
}