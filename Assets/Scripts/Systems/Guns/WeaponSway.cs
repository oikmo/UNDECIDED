using System;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Sway Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float sensitivityMultiplier;
    Quaternion rotationY;
    Quaternion rotationX;

    private Quaternion refRotation;

    private float xRotation;
    private float yRotation;

    private void Update()
    {
        // get mouse input
        if(GameHandler.Instance !=null)
        {
            if(GameHandler.Instance.oMenu != null)
            {
                if (GameHandler.Instance.oMenu.sens != 0)
                {
                    float mouseY = GameHandler.Instance.playerInput.Player.Look.ReadValue<Vector2>().y * sensitivityMultiplier;
                    rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
                    float mouseX = GameHandler.Instance.playerInput.Player.Look.ReadValue<Vector2>().x * sensitivityMultiplier;
                    rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
                }
            }
            
        }
        

        if(rotationX == Quaternion.AngleAxis(0, Vector3.right))
        {
            rotationX = Quaternion.AngleAxis(1, Vector3.right);
        }
        if (rotationY == Quaternion.AngleAxis(0, Vector3.up))
        {
            rotationY = Quaternion.AngleAxis(1, Vector3.up);
        }

        Quaternion targetRotation = rotationX * rotationY;
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, speed * Time.deltaTime);
    }
}