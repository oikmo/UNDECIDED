using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] Object[] objects;

    void Start()
    {
        DontDestroyOnLoad(this);

        for(int i = 0; i<objects.Length; i++) 
        {
            if(objects[i] != null) 
            {
                DontDestroyOnLoad(objects[i]);
            }
        }
    }
}