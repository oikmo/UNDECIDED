using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public bool grounded = false;

    private void OnTriggerEnter(Collider collision) {
        grounded = true;
    }

    private void OnTriggerStay(Collider other) {
        grounded = true;
    }

    private void OnTriggerExit(Collider collision) {
        grounded = false;
    }
}
