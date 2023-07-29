using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunningAdvanced : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    private bool upwardsRunning;
    private bool downwardsRunning;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    private PlayerMovementAdvanced pm;
    private LedgeGrabbing lg;

    private void Start()
    {
        pm = GameHandler.Instance.pm;
        lg = GetComponent<LedgeGrabbing>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        upwardsRunning = GameHandler.Instance.sprinting;
        downwardsRunning = GameHandler.Instance.crouching;

        // State 1 - Wallrunning
        if((wallLeft || wallRight) && GameHandler.Instance.verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
            {
                pm.donttilt = true;
                StartWallRun();
            }
                

            // wallrun timer
            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if(wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            // wall jump
            if (GameHandler.Instance.jumping) WallJump();
        }

        // State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
            {
                
                StopWallRun();
            }
                
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        pm.rb.velocity = new Vector3(pm.rb.velocity.x, 0f, pm.rb.velocity.z);

        // apply camera effects
        if(pm.pCam != null)
        {
            SettingsData data = SaveSystem.LoadSettings();
            pm.pCam.DoFov(data.fov + 20f);
            if (wallLeft) pm.pCam.DoTilt(-5f);
            if (wallRight) pm.pCam.DoTilt(5f);
        }
        
    }

    private void WallRunningMovement()
    {
        pm.rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        pm.rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards/downwards force
        if (upwardsRunning)
            pm.rb.velocity = new Vector3(pm.rb.velocity.x, wallClimbSpeed, pm.rb.velocity.z);
        if (downwardsRunning)
            pm.rb.velocity = new Vector3(pm.rb.velocity.x, -wallClimbSpeed, pm.rb.velocity.z);

        // push to wall force
        if (!(wallLeft && GameHandler.Instance.horizontalInput > 0) && !(wallRight && GameHandler.Instance.horizontalInput < 0))
            pm.rb.AddForce(-wallNormal * 100, ForceMode.Force);

        // weaken gravity
        if (useGravity)
            pm.rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;

        // reset camera effects
        if(pm.pCam != null)
        {
            pm.pCam.DoFov(90f);
        }
        
        //cam.DoTilt(0f);

        pm.donttilt = false;
    }

    private void WallJump()
    {
        if (lg.holding || lg.exitingLedge) return;

        // enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        pm.rb.velocity = new Vector3(pm.rb.velocity.x, 0f, pm.rb.velocity.z);
        pm.rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
