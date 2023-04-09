using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public Slider staminaBar;
    private float stamina;

    public void Decrease(float number)
    {
        staminaBar.value = (int)Math.Round(stamina);
    }

    public void Increase(float number)
    {

    }
}
