using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    public int maxHealth;
    public float curhealth = 100f;
    bool doDamageIdiot;
    float curDamage;
    [SerializeField] float fallThresholdVelocity = 5f;
    [SerializeField] Animator camAnim;

    // Update is called once per frame
    void Update()
    {
        if(GameHandler.Instance != null)
        {
            if (GameHandler.Instance.vHealth != null)
            {
                GameHandler.Instance.vHealth.alpha = 1.0f - (curhealth / 100f);
            }

            if (GameHandler.Instance.pm != null)
            {
                if (!GameHandler.Instance.pm.grounded)
                {
                    //print("do damage : " + (pm.rb.velocity.y < -fallThresholdVelocity));
                    if (GameHandler.Instance.pm.rb.velocity.y < -fallThresholdVelocity)
                    {
                        print("balls");
                        curDamage = Mathf.Abs(GameHandler.Instance.pm.rb.velocity.y) + fallThresholdVelocity;
                        doDamageIdiot = true;
                        //if trigger false turn of animator!!!
                    }
                }
                else
                {
                    if (doDamageIdiot)
                    {
                        takeDmg(curDamage);
                        doDamageIdiot = false;
                    }
                }
            }
        }

        if(curhealth == 0 && !GameHandler.Instance.playerAlreadyDead)
        {
            GameHandler.Instance.Die();
        }

        if(GameHandler.Instance.playerAlreadyDead && curhealth > 0)
        {
            GameHandler.Instance.ResetDie();
        }
    }

    public void takeDmg(float dmg)
    {
        camAnim.SetTrigger("fallDamage");
        
        if (curhealth <= maxHealth && curhealth != 0)
        {
            print(curhealth - dmg);

            if (curhealth - dmg < 0)
            {
                curhealth = 0;
                Cicero.Instance.DisplayText("HEY! HEY! YOU OKAY!?!?! oh shit he dead.");
            }
            else
            {
                curhealth -= dmg;
                Cicero.Instance.DisplayText("You have taken " + (int)dmg + " points of damage.");
            }
        }
    }

    public void giveHealth(float heal)
    {
        if (curhealth < maxHealth && !(curhealth >= maxHealth))
        {
            curhealth += heal;
            Cicero.Instance.DisplayText("You have taken " + (int) heal + " points of damage.");
        }
    }
}
