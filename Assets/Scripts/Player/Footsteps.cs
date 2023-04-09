using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource footstepsSound, crouchSound, sprintSound, slidingSound, wind;

    void Update()
    {
        if (GameHandler.Instance != null)
        {
            if (GameHandler.Instance.pm != null)
            {
                //print(GameHandler.Instance.pm.rb.velocity.sqrMagnitude);
                if (!GameHandler.Instance.pm.grounded || !GameHandler.Instance.pm.grounded && GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.idle || GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.idle || GameHandler.Instance.paused)
                {
                    footstepsSound.volume = 0;
                    sprintSound.volume = 0;
                    slidingSound.volume = 0;
                    footstepsSound.enabled = false;
                    crouchSound.enabled = false;
                    sprintSound.enabled = false;
                    slidingSound.enabled = false;

                }
                if (GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.idle)
                {
                    footstepsSound.enabled = false;
                    sprintSound.enabled = false;
                    slidingSound.enabled = false;
                    wind.enabled = false;
                    crouchSound.enabled = false;
                }
                else
                {
                    if (GameHandler.Instance.pm.grounded)
                    {
                        if (GameHandler.Instance.pm.definitelyNotIdle())
                        {
                            if (GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.walking)
                            {
                                footstepsSound.enabled = true;
                                crouchSound.enabled = false;
                                sprintSound.enabled = false;
                                slidingSound.enabled = false;
                                wind.enabled = false;
                                footstepsSound.volume = 1;
                            }

                            if (GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.crouching)
                            {
                                footstepsSound.enabled = false;
                                crouchSound.enabled = true;
                                sprintSound.enabled = false;
                                slidingSound.enabled = false;
                                wind.enabled = false;
                                crouchSound.volume = 1;
                            }

                            if (GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.sprinting)
                            {
                                footstepsSound.enabled = false;
                                crouchSound.enabled = false;
                                sprintSound.enabled = true;
                                slidingSound.enabled = false;
                                wind.enabled = true;
                                wind.volume = 0.65F;
                                sprintSound.volume = 1;
                            }
                            if (GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.sliding)
                            {
                                footstepsSound.enabled = false;
                                crouchSound.enabled = false;
                                sprintSound.enabled = false;
                                slidingSound.enabled = true;
                                wind.enabled = true;
                                wind.volume = 0.75F;
                                slidingSound.volume = 1;
                            }
                        }

                    }

                    if ((GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.air && GameHandler.Instance.pm.rb.velocity.y <= -15) || (GameHandler.Instance.pm.state == PlayerMovementAdvanced.MovementState.groundslam) && !GameHandler.Instance.pm.grounded)
                    {
                        footstepsSound.enabled = false;
                        crouchSound.enabled = false;
                        sprintSound.enabled = false;
                        slidingSound.enabled = false;
                        wind.enabled = true;
                        wind.volume = 1;
                    }
                }
            }
        }


    }
}
