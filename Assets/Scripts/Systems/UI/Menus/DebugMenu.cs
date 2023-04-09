using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    public GameObject gO;
    public bool isDebug = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3) && !GameHandler.Instance.paused)
        {
            isDebug = !isDebug;

        }
        
        gO.SetActive(isDebug);
    }
}
