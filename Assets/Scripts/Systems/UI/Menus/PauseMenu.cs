using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pMenu, resumebutton, credit;
    Button load;
    public bool isPaused;

    float cooldown = 1f;
    [SerializeField] float lastPressTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        pMenu.SetActive(false);
        load = pMenu.transform.Find("Load").GetComponent<Button>();
        //credit = transform.Find("Credits").GetComponent<GameObject>();
        load.onClick.AddListener(GameHandler.Instance.pStuff.loadPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (pMenu.activeSelf)
        {
            isPaused = true;
        }
        if (!pMenu.activeSelf)
        {
            isPaused = false;
        }
        if (load == null)
        {
            load = pMenu.transform.Find("Load").GetComponent<Button>();
        }

        if(pMenu.activeSelf != credit.activeSelf)
        {
            credit.SetActive(pMenu.activeSelf);
        }

        if(GameHandler.Instance != null)
        {
            if(GameHandler.Instance.playerInput != null)
            {
                if (GameHandler.Instance.pausing && !GameHandler.Instance.paused)
                {
                    float currentTime = Time.time;

                    float diffSecs = currentTime - lastPressTime;
                    if (diffSecs >= cooldown)
                    {
                        lastPressTime = currentTime;
                        if (!isPaused)
                        {
                            PauseGame();
                        }
                        else
                        {
                            ResumeGame();
                        }
                    }
                }
            }
        }
        
    }

    public void PauseGame()
    {
        isPaused = true;
        pMenu.SetActive(true);
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        pMenu.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        SceneManager.LoadScene("MainMenus");
    }

    public void OpenOptions()
    {
        if(GameHandler.Instance != null)
        {
            if(GameHandler.Instance.oMenu != null)
            {
                GameHandler.Instance.oMenu.gO.SetActive(true);
                GameHandler.Instance.oMenu.isOptions = true;

                isPaused = false;
                pMenu.SetActive(false);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
