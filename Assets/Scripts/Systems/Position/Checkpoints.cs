using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    private PlayerStuff ps;
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            switch (this.name)
            {
                case "checkpoint1":
                    ps = other.GetComponent<PlayerStuff>(); ps.checkpoint = 1;
                    break;
                case "checkpoint2":
                    ps = other.GetComponent<PlayerStuff>(); ps.checkpoint = 2;
                    break;
            }
            
        }
    }
}