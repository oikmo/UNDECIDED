using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public GameObject Object;
    public float distance = 0.3f;
    public Vector3 player;

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player") != null) 
        {
            player = GameObject.Find("Player").transform.position;

            if (Vector3.Distance(transform.position, player) <= distance)
            {
                Object.SetActive(true);
            } else
            {
                Object.SetActive(false);
            }

            if (Object != null)
            {
                if(GameObject.Find("PlayerCam")){
                    Object.transform.LookAt(GameObject.Find("PlayerCam").transform.position);
                    Object.transform.Rotate(0, 180, 0);
                }
                
            } 
        }
        
    }
}
