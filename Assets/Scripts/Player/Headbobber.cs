using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbobber : MonoBehaviour
{
    public float bobSpeed = 1f;
    public float maxHorizontalBob = 0.08f, maxHorizontalBobWhileRunning = 0.2f, maxHorizontalBobWhileCrouching;
    public float maxVerticalBob = 0.05f, maxVerticalBobWhileRunning = 0.1f, maxVerticalBobWhileCrouching;
    public float eyeHeightRatio = 0.9f; // the ratio of the height of the cam to the height of the player
    private Vector3 parentLastPostion; //the last position of the player
    private float bobStepCounter = 0f;
    [SerializeField] private PlayerMovementAdvanced pm;

    void Awake()
    {
        parentLastPostion = transform.parent.position;
    }

    void Update()
    {
        #region Motion Bob
        if (pm.grounded && pm.rb.velocity.magnitude > 0)
        {
            if (pm.state == PlayerMovementAdvanced.MovementState.walking)
            {
                bobStepCounter += Vector3.Distance(parentLastPostion, transform.parent.position) * bobSpeed;
                float posX, posY;
                posX = Mathf.Sin(bobStepCounter) * maxHorizontalBob;
                posY = (Mathf.Cos(bobStepCounter * 2) * maxVerticalBob * -1) + (transform.localScale.y * eyeHeightRatio) - (transform.localScale.y / 2);
                transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);
                parentLastPostion = transform.parent.position;
            }
            else if(pm.state == PlayerMovementAdvanced.MovementState.sprinting)
            {
                bobStepCounter += Vector3.Distance(parentLastPostion, transform.parent.position) * bobSpeed;
                float posX, posY;
                posX = Mathf.Sin(bobStepCounter) * maxHorizontalBobWhileRunning;
                posY = (Mathf.Cos(bobStepCounter * 2) * maxVerticalBobWhileRunning * -1) +(transform.localScale.y * eyeHeightRatio) - (transform.localScale.y / 2);
                transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);
                parentLastPostion = transform.parent.position;
            }
            else if (pm.state == PlayerMovementAdvanced.MovementState.crouching)
            {
                bobStepCounter += Vector3.Distance(parentLastPostion, transform.parent.position) * bobSpeed;
                float posX, posY;
                posX = Mathf.Sin(bobStepCounter) * maxHorizontalBobWhileCrouching;
                posY = (Mathf.Cos(bobStepCounter * 2) * maxVerticalBobWhileCrouching * -1) + (transform.localScale.y * eyeHeightRatio) - (transform.localScale.y / 2);
                transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);
                parentLastPostion = transform.parent.position;
            }
        }
        #endregion
    }

}