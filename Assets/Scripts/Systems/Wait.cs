using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    public float wait_time = 5f;

    void Start()
    {
        Screen.SetResolution(1280, 720, false);
        StartCoroutine(WaitIntro());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene("MainMenus");
        }
    }

    IEnumerator WaitIntro()
    {
        yield return new WaitForSeconds(wait_time);

        SceneManager.LoadScene("MainMenus");
    }
}
