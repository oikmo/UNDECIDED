using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovementAdvanced : MonoBehaviour {
    [Header("Camera Effects")]
    GameObject sLines;
    public PlayerCam pCam;
    public int zoom = 120;
    public int normal = 90;
    public float smooth = 5;

    Vector3 boxSize = new Vector3(1, 0.48f, 1);
    [SerializeField] float distance = 1;

    [Header("Movement")]
    public float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;
    public float climbSpeed;
    public float vaultSpeed;
    public float airMinSpeed;
    public float staminaDecrease = 0.1f;
    public float staminaIncrease = 0.1f;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Stamina")]
    public float stamina = 100f;

    public float normalIncrease = 0.35f;
    public float airIncrease = 0.15f;
    public float lockIncrease = 1f;

    public float sprintingDecrease = 0.2f;
    public float climbingDecrease = 0.3f;
    public float wallrunningDecrease = 0.2f;

    [Header("Effects")]
    public float number;
    bool isVignetteShown = false;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("References")]
    public Climbing climbingScript;
    private ClimbingDone climbingScriptDone;
    public Transform orientation;

    Vector3 moveDirection;

    public Rigidbody rb;

    public MovementState state;
    public enum MovementState {
        idle,
        freeze,
        walking,
        sprinting,
        wallrunning,
        groundslam,
        climbing,
        vaulting,
        crouching,
        sliding,
        air
    }
    bool staminaGOGO;
    public bool staminaLock;
    public bool sprinting;
    public bool idle;
    public bool sliding;
    public bool crouching;
    public bool wallrunning;
    public bool climbing;
    public bool vaulting;
    public bool freeze;
    public bool restricted;

    Vector3 offsetHeight = Vector3.zero;
    Vector3 spherePos = Vector3.zero;

    [HideInInspector]
    public bool previouslyGrounded;

    [SerializeField] TextMeshProUGUI text_speed;
    [SerializeField] TextMeshProUGUI text_mode;
    [SerializeField] TextMeshProUGUI text_position;
    [SerializeField] TextMeshProUGUI text_velocity;

    public bool donttilt;
    Vector2 WASDinput = Vector2.zero;
    bool keepMomentum;

    [SerializeField] WallChecker wChecker;
    [SerializeField] GroundChecker gChecker; //quick hack to fix grounded issue :)

    private void Start() {
        climbingScriptDone = GetComponent<ClimbingDone>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        //charControl = GetComponent<CharacterController>();

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update() {


        if (GameHandler.Instance != null) {
            if (GameHandler.Instance.vStamina != null) {
                GameHandler.Instance.vStamina.alpha = 1.0f - (stamina / 100f);
            }

            if (sLines == null) {
                if (GameObject.Find("SpeedLines").GetComponent<ParticleSystem>().gameObject != null) {
                    sLines = GameObject.Find("SpeedLines").GetComponent<ParticleSystem>().gameObject;
                }

            }
        if (pCam == null) {
                pCam = GameObject.Find("PlayerCam").GetComponent<PlayerCam>();
            }

            sprinting = GameHandler.Instance.sprinting && (GameHandler.Instance.horizontalInput != 0 || GameHandler.Instance.verticalInput != 0) && stamina > 0 && !staminaLock;
            MyInput();
            SpeedControl();
            StateHandler();
            TextStuff();

            // handle drag
            if (grounded) { rb.drag = groundDrag; } else { rb.drag = 0; }
            if (stamina < 10) {
                if (stamina == 0) {
                    staminaLock = true;
                    staminaIncrease = lockIncrease;
                }
            } else if (stamina == 100) {
                staminaLock = false;
                staminaIncrease = normalIncrease;
            }

            if (pCam != null) {
                if (moveSpeed >= 10 && (state == MovementState.sprinting || state == MovementState.wallrunning) && stamina > 0 && !staminaLock) {
                    SettingsData data = SaveSystem.LoadSettings();

                    if (data != null) {
                        pCam.DoFov(data.fov + 20);
                    }

                    if (sLines != null) {
                        sLines.SetActive(true);
                    }

                } else if (moveSpeed >= 10 && state == MovementState.sliding) {
                    SettingsData data = SaveSystem.LoadSettings();

                    if (data != null) {
                        pCam.DoFov(data.fov + 30);
                    }

                    if (sLines != null) {
                        sLines.SetActive(true);
                    }
                } else if (moveSpeed <= 10 && (state != MovementState.sprinting || state != MovementState.wallrunning)) {
                    SettingsData data = SaveSystem.LoadSettings();

                    if (data != null) {
                        pCam.DoFov(data.fov);
                    }

                    if (sLines != null) {
                        sLines.SetActive(false);
                    }
                }
            }

            if (!GameHandler.Instance.paused) {
                if (GameHandler.Instance.horizontalInput == 0 && GameHandler.Instance.verticalInput == 0 && grounded && !notIdle()) { idle = true; } else { idle = false; }
                if (staminaGOGO) { if (stamina > 0 || stamina != 0) { stamina -= staminaDecrease * (60f * Time.deltaTime); } }
                if (!staminaGOGO && idle || crouching || state == MovementState.walking || state == MovementState.air) { if (stamina < 100 || stamina != 100) { stamina += staminaIncrease * (60f * Time.deltaTime); } }
                if (stamina > 100) { stamina = 100; }
                if (stamina < 0) { stamina = 0; }

                pCam.particle.gameObject.SetActive(state == MovementState.sliding);
            }

        }
    }

    private void FixedUpdate() {
        previouslyGrounded = grounded;
        //grounded = charControl.isGrounded;

        offsetHeight = new Vector3(0.0f, .1f + playerHeight / 2, 0.0f);
        spherePos = transform.position - offsetHeight;
        grounded = gChecker.grounded;
        MovePlayer();

        Vector3 vel = rb.velocity;
        if ((int)vel.magnitude == 0) {
            rb.velocity = new Vector3(moveDirection.x * desiredMoveSpeed, rb.velocity.y, moveDirection.z * desiredMoveSpeed);
        }
        //to prevent player from sticking to walls :)
        if (wChecker.touchingWall && !grounded && !wallrunning) {
            moveDirection = Vector3.zero;
            Vector3 tempVel = rb.velocity;
            tempVel.x = 0;
            tempVel.y = rb.velocity.y;
            tempVel.z = 0;
            rb.velocity = tempVel;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(spherePos, 1f);
    }

    private void MyInput() {
        if (GameHandler.Instance != null) {
            if (!GameHandler.Instance.paused) {
                if (GameHandler.Instance.playerInput != null) {

                    if (!donttilt) {
                        if (pCam != null) {
                            if (Math.Round(GameHandler.Instance.horizontalInput) < 0) {
                                pCam.DoTilt(2);
                            } else if (Math.Round(GameHandler.Instance.horizontalInput) > 0) {
                                pCam.DoTilt(-2);
                            }
                            if (state == MovementState.wallrunning || GameHandler.Instance.horizontalInput == 0) {
                                pCam.DoTilt(0);
                            }
                        }

                    }

                    // when to jump
                    if (GameHandler.Instance.jumping && readyToJump && grounded) {
                        readyToJump = false;

                        Jump();

                        Invoke(nameof(ResetJump), jumpCooldown);
                    }

                    // start crouch
                    if (GameHandler.Instance.crouching && state != MovementState.sprinting && grounded) {
                        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                        //rb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
                        playerHeight = 0.5f;
                        crouching = true;
                    } else {
                        transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                        playerHeight = startYScale;
                        crouching = false;
                    }
                }



                //groundslam
                /*if (!grounded && (Input.GetButtonDown("Crouch")) && state != MovementState.crouching && state == MovementState.air)
                {
                    rb.AddForce(Vector3.down * 20f, ForceMode.Impulse);
                    groundslam = true;
                }*/
            }
        }
    }
    ///<summary>
    /// function that returns true if player is not idle via input
    ///</summary>
    public bool definitelyNotIdle() => GameHandler.Instance.horizontalInput != 0 || GameHandler.Instance.verticalInput != 0;
    ///<summary>
    /// function that returns true if player is not idle via state
    ///</summary>
    public bool notIdle() => sprinting || sliding || crouching || wallrunning || climbing || vaulting;
    private void StateHandler() {

        // Mode - Freeze
        if (freeze) {
            if (!staminaLock) { staminaIncrease = normalIncrease; }
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
            desiredMoveSpeed = 0f;
            staminaGOGO = false;
        } else if (idle) {
            if (!staminaLock) { staminaIncrease = normalIncrease; }
            state = MovementState.idle;
            desiredMoveSpeed = 7f;
            staminaGOGO = false;
        }

          // Mode - Vaulting
          else if (vaulting) {
            if (!staminaLock) { staminaIncrease = normalIncrease; }
            state = MovementState.vaulting;
            desiredMoveSpeed = vaultSpeed;
            idle = false;
            staminaGOGO = false;
        }

          // Mode - Climbing
          else if (climbing) {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
            idle = false;
            staminaDecrease = climbingDecrease;
            staminaGOGO = true;
        }

          // Mode - Wallrunning
          else if (wallrunning) {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
            idle = false;
            staminaDecrease = wallrunningDecrease;
            staminaGOGO = true;
        }

          // Mode - Sliding
          else if (sliding) {
            if (!staminaLock) { staminaIncrease = normalIncrease; }
            state = MovementState.sliding;

            // increase speed by one every second
            if (OnSlope() && rb.velocity.y < 0.1f) {
                desiredMoveSpeed = slideSpeed;
            } else
                desiredMoveSpeed = sprintSpeed;

            idle = false;
            staminaGOGO = false;
        }

          // Mode - Crouching
          else if (crouching) {
            if (!staminaLock) { staminaIncrease = normalIncrease; }
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
            staminaGOGO = false;
            idle = false;
        }

          // Mode - Sprinting
          else if (sprinting) {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
            staminaGOGO = true;
            staminaDecrease = sprintingDecrease;
            idle = false;
        }

          // Mode - Walking
          else if (grounded) {
            if (!staminaLock) { staminaIncrease = normalIncrease; }
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
            idle = false;
            staminaGOGO = false;
        }

          // Mode - Air
          else {
            if (!staminaLock) { staminaIncrease = airIncrease; }
            //print(rb.velocity.y);
            state = MovementState.air;
            idle = false;
            staminaGOGO = false;
            if (moveSpeed < airMinSpeed)
                desiredMoveSpeed = airMinSpeed;
        }

        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (desiredMoveSpeedHasChanged) {
            if (keepMomentum) {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            } else {
                moveSpeed = desiredMoveSpeed;
            }
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;

        // deactivate keepMomentum
        //if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.1f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed() {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;
        while (time < difference) {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope()) {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            } else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }
        keepMomentum = false;

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer() {
        if (climbingScript.exitingWall) return;
        if (climbingScriptDone.exitingWall) return;
        if (restricted) return;
        

        // calculate movement direction
        moveDirection = orientation.forward * GameHandler.Instance.verticalInput + orientation.right * GameHandler.Instance.horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope) {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded && !wChecker.touchingWall)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        if (!wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl() {
        //print("state != MovementState.air || grounded " + (state != MovementState.air || grounded));
        //print(grounded);
        // limiting speed on slope
        if (OnSlope() && !exitingSlope) {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        // limiting speed on ground or in air
        else {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            Vector3 tempVel = rb.velocity;
            // limit velocity if needed
            if ((state != MovementState.air || grounded) && flatVel.magnitude > moveSpeed) {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                tempVel.y = rb.velocity.y;
                tempVel.x = Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed);
                tempVel.y = rb.velocity.y;
                tempVel.z = Mathf.Clamp(rb.velocity.z, -moveSpeed, moveSpeed);
                tempVel.y = rb.velocity.y;
                rb.velocity = tempVel;
                tempVel.y = rb.velocity.y;
                rb.AddForce(-new Vector3(limitedVel.x, -rb.velocity.y, limitedVel.z));
                tempVel.y = rb.velocity.y;
                rb.velocity = tempVel;

            }
        }
    }

    public void Jump() {

        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump() {
        readyToJump = true;

        exitingSlope = false;

    }

    public bool OnSlope() {
        
        if (Physics.Raycast(transform.position, -transform.up, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction) {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void TextStuff() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (OnSlope())
            text_speed.SetText("Speed: " + Round(rb.velocity.magnitude, 1) + " / " + Round(moveSpeed, 1));

        else
            text_speed.SetText("Speed: " + Round(flatVel.magnitude, 1) + " / " + Round(moveSpeed, 1));

        text_mode.SetText(state.ToString());

        text_position.SetText("X : " + (int)orientation.position.x + "\n" + "Y : " + (int)orientation.position.y + "\n" + "Z : " + (int)orientation.position.z + "\n");

        text_velocity.SetText("X : " + rb.velocity.x.ToString("0.00") + "\n" + "Y : " + rb.velocity.y.ToString("0.00") + "\n" + "Z : " + rb.velocity.z.ToString("0.00") + "\n");
    }

    public static float Round(float value, int digits) {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public Vector3 GetRBVelocity() {
        return rb.velocity;
    }
}
