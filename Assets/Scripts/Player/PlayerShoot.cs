using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    public static Action shootInput;
    public static Action reloadInput;

    private void Update()
    {
        if (GameHandler.Instance.firing) {shootInput?.Invoke();}
        if (GameHandler.Instance.reloading) {reloadInput?.Invoke();}
    }
}
