using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public float magnitude;

    private AudioSource impact;
    private PlayerMovementAdvanced pm;

    // Start is called before the first frame update
    void Start()
    {
        impact = GetComponent<AudioSource>();
        pm = GetComponent<PlayerMovementAdvanced>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.relativeVelocity.magnitude >= magnitude && pm.grounded)
        {
            impact.enabled = true;
        } else
        {
            impact.enabled = false;
        }
    }
}
