using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using TMPro;
using System;

public class WeaponSwitching : MonoBehaviour {
    private GameObject[] weapons;
    public void Update() {
        if(GameHandler.Instance.playerInventory.GetSelectedItem()) {
            for (int i = 0; i < weapons.Length; )
                switch (GameHandler.Instance.playerInventory.GetSelectedItem().itemName) {
                    case "Pistolinie":

                        break;
                }

        }
    }
}