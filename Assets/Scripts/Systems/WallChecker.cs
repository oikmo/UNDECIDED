using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallChecker : MonoBehaviour
{
    public bool touchingWall = false;

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == 9) {
            touchingWall = true;
        }
    }

    private void OnTriggerExit(Collider collision) {
        if (collision.gameObject.layer == 9) {
            touchingWall = false;
        }
    }
}
