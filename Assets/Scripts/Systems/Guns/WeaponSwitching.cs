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
    public void Update() {
        if (GameHandler.Instance.playerInventory.GetSelectedItem()) {
            //print("TRUE!");
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
        }

    }

    void SetWeapon(int index) {
        weapons[index].SetActive(true);
        for(int i = 0; i<weapons.Length; i++) {
            if(i==index) { return; }
            weapons[i].SetActive(false);
        }
    }
}