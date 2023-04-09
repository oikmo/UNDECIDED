using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearFromCam : MonoBehaviour
{
    private Renderer target;
    public Renderer smallTarget;

    void Start()
    {
        target = gameObject.GetComponent<Renderer>();
        target.enabled = false;
    }

    void Update()
    {
        if(smallTarget.isVisible)
        {
            target.enabled = true;
            //print("hello");
        } else
        {
            target.enabled = false;
            //print("bye");
        }
    }

}
