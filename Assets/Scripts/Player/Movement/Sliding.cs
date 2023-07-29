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
            if (GameHandler.Instance.sliding && !slideLock)
                StartSlide();

            if (!GameHandler.Instance.sliding && pm.sliding || !pm.definitelyNotIdle())
                StopSlide();
        }
        
    }

    private void FixedUpdate() {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        if (pm.wallrunning) return;
        //if (!pm.doublejump) return;

        pm.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        pm.rb.velocity = new Vector3(pm.rb.velocity.x, -2, pm.rb.velocity.z);
        if(pm.playerHeight != 0.5f) {
            inputDirection = playerObj.forward;
        }
        pm.playerHeight = 0.5f;
       
    }

    private void SlidingMovement()
    {
        pm.rb.velocity = new Vector3(pm.rb.velocity.x, -2, pm.rb.velocity.z);

        // sliding normal
        if (!pm.OnSlope() || pm.rb.velocity.y > -0.1f) {
            pm.rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        }

        // sliding down a slope
        else {
            pm.rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }
    }

    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        pm.playerHeight = 2f;
    }
}