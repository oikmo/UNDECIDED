using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [Header("menus")]
    public bool toPrevent1;
    public GameObject menu1, menu2, menu3;
    [Header("menu buttons")]
    public bool toPrevent2;
    public GameObject menu1btn, menu2btn, menu3btn;
    bool is1, is2, is3;
    [SerializeField] LevelLoader loader;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if(GameObject.Find("Important Objects") != null)
        {
            Destroy(GameObject.Find("Important Objects"));
        }

        if(!File.Exists(Application.persistentDataPath+ "/save.txt"))
        {
            File.Create(Application.persistentDataPath+ "/save.txt");
        }

        if(menu1.activeSelf) 
        {
            menu1.SetActive(true);
            menu2.SetActive(false);
            menu3.SetActive(false);
            if(!is1)
            {
                EventSystem.current.SetSelectedGameObject(menu1btn);
                is1 = true;
            }
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetButton("Mouse X") || Input.GetButton("Mouse Y"))
            {
                EventSystem.current.SetSelectedGameObject(menu1btn);
            }
            is2 = false;
            is3 = false;
        }
        
        if(menu2.activeSelf) 
        {
            menu1.SetActive(false);
            menu2.SetActive(true);
            menu3.SetActive(false);
            if(!is2)
            {
                    
                EventSystem.current.SetSelectedGameObject(menu2btn);
                is2 = true;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetButton("Mouse X") || Input.GetButton("Mouse Y"))
            {
                EventSystem.current.SetSelectedGameObject(menu2btn);
            }
            is1 = false;
            is3 = false;
        }
        if (menu3.activeSelf)
        {
            menu1.SetActive(false);
            menu2.SetActive(false);
            menu3.SetActive(true);
            if (!is3)
            {
                EventSystem.current.SetSelectedGameObject(menu3btn);
                is3 = true;
            }

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetButton("Mouse X") || Input.GetButton("Mouse Y"))
            {
                EventSystem.current.SetSelectedGameObject(menu3btn);
            }
            is1 = false;
            is2 = false;

            if(File.Exists(Application.persistentDataPath + "/save.txt"))
            {
                File.WriteAllText(Application.persistentDataPath + "/save.txt", "false");
            }
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }


    public void LoadSave()
    {
        if(File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            File.WriteAllText(Application.persistentDataPath + "/save.txt", "true");
        }
        SaveSystem.setName("save");
        PlayerData data = SaveSystem.LoadPlayer();
        print(GameHandler.NameFromIndex(data.level + 2));
        loader.LoadLevel(GameHandler.NameFromIndex(data.level + 2));
    }
}
