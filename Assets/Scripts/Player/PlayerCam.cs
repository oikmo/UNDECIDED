using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("References")]
    public float sensX;
    public float sensY;
    public Transform orientation;
    public Transform camHolder;
    public Transform camThingy;
    public Transform particle;
    public Camera cam;
    public Vector3 test;
    [Header("Rotation")]
    public float xRotation;
    public float yRotation;
    float mouseX = 0;
    float mouseY = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (GameHandler.Instance != null) 
        {
            if (!GameHandler.Instance.paused && !GameHandler.Instance.isInventory)
            {
                
                // get mouse input
                if (GameHandler.Instance.playerInput != null && GameHandler.Instance.oMenu != null)
                {
                    if(GameHandler.Instance.curDevice == "keyboard&mouse")
                    {
                        mouseX = GameHandler.Instance.mouseX * Time.deltaTime * GameHandler.Instance.oMenu.sens;
                        mouseY = GameHandler.Instance.mouseY * Time.deltaTime * GameHandler.Instance.oMenu.sens;
                    } 
                    else if(GameHandler.Instance.curDevice == "gamepad")
                    {
                        mouseX = GameHandler.Instance.mouseX * Time.deltaTime * (GameHandler.Instance.oMenu.sens * 2);
                        mouseY = GameHandler.Instance.mouseY * Time.deltaTime * (GameHandler.Instance.oMenu.sens * 2);
                    }
                }

                particle.rotation = Quaternion.Euler(0, yRotation, 0);;

                //print("mouseX : " + mouseX + ", mouseY : " + mouseY);
                yRotation += mouseX;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -80f, 80f);

                // rotate cam and orientation
                camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);

                orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            }
        }
        
        camHolder.position = camThingy.position;
    }

    public void DoFov(float endValue)
    {
        //print("FOV " + cam.fieldOfView + ", endValue " + endValue);
        if((int)cam.fieldOfView != (int)endValue)
        {
            cam.DOFieldOfView(endValue, 0.25f);
        }
        
    }

    public void SetFOV(float value)
    {
        cam.fieldOfView = value;
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}