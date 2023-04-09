using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private PlayerMovementAdvanced pm;

    [Header("Sliding")]
    public float slideForce;
    public float slideYScale;
    private float startYScale;

    Vector3 inputDirection = Vector3.zero;

    bool slideLock = false;

    private void Start()
    {
        pm = GameHandler.Instance.pm;

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        if(GameHandler.Instance != null)
        {
            if (GameHandler.Instance.crouching && pm.sprinting && pm.grounded && !slideLock)
                StartSlide();

            if (!GameHandler.Instance.crouching && pm.sliding || !pm.grounded || !pm.definitelyNotIdle())
                StopSlide();
        }
        
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        if (pm.wallrunning) return;
        //if (!pm.doublejump) return;

        pm.sliding = true;
        if(playerObj.localScale.y != slideYScale)
        {
            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        }

        pm.rb.velocity = new Vector3(pm.rb.velocity.x, -2, pm.rb.velocity.z);
    }

    private void SlidingMovement()
    {

        pm.rb.velocity = new Vector3(pm.rb.velocity.x, -2, pm.rb.velocity.z);

        if (GameHandler.Instance != null)
        {
            inputDirection = orientation.forward * GameHandler.Instance.verticalInput + orientation.right * GameHandler.Instance.horizontalInput;
        }
        
        slideForce -= 10f * Time.deltaTime;

        // sliding normal
        if (!pm.OnSlope() || pm.rb.velocity.y > -0.1f)
        {
            
            pm.rb.AddForce((inputDirection.normalized  * -2f) * slideForce, ForceMode.Force);
        }
        // sliding down a slope
        else
        {
            //inputDirection = inputDirection * 0.95f * Time.deltaTime;
            pm.rb.AddForce(pm.GetSlopeMoveDirection((inputDirection) * -2f) * slideForce, ForceMode.Force);
        }

        //print(float.Parse((inputDirection.normalized.x * slideForceLimit).ToString("0")) + float.Parse((inputDirection.normalized.z * slideForceLimit).ToString("0")));

        if(slideForce <= -50f)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;
        
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);

        slideForce = 0;
        
    }
}
