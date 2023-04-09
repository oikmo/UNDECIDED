using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchScene : MonoBehaviour
{
    [SerializeField] string scene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(scene);
        }
    }
}
