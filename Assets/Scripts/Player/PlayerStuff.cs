using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEditor;

public class PlayerStuff : MonoBehaviour
{
    [HideInInspector] public int version = 1;

    [Header("Player")]
    public InventoryObject inventory;
    public GameObject player;
    public GameObject errorNotice;

    [Header("Info")]
    public int level = 0;
    public int checkpoint = 0;
    public float timeRemaining = 3;

    void Awake()
    {
        if(!Directory.Exists(Application.persistentDataPath + "/Saves/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }

        if(File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            if(File.ReadAllText(Application.persistentDataPath + "/save.txt") == "true")
            {
                loadPlayer();
            }
            else if(File.ReadAllText(Application.persistentDataPath + "/save.txt") == "false")
            {
                //just dont do anything :)
            }
            else
            {
                loadPlayer(); //just in case someone edits the file and yeah
            }
        }
    }

    void Update()
    {
        if (inventory.isError)
        {
            errorNotice.SetActive(true);
            if (Input.anyKey)
            {
                inventory.isError = false;
            }
        }
        else
        {
            errorNotice.SetActive(false);
        }

        if(level != GameHandler.GetActiveScene())
        {
            level = GameHandler.GetActiveScene();
        }

        //print(level);
    }

    public void savePlayer()
    {
        SaveSystem.setName("save");
        SaveSystem.SavePlayer();
        inventory.Save();
    }

    public void loadPlayer()
    {
        SaveSystem.setName("save");
        PlayerData data = SaveSystem.LoadPlayer();
        version = data.version;
        GameHandler.Instance.pHealth.curhealth = data.health;
        checkpoint = data.checkpoint;
        GameHandler.Instance.HECHEATED = data.HECHEATED;
        inventory.Load();
        
        GameObject.Find("PauseMenuHolder").GetComponent<PauseMenu>().ResumeGame();

        switch (data.level) 
            {
                case 0:
                    GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("Training");
                    break;
                case 1:
                    GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel("ch1_p1");
                    break;
            }
        level = data.level;

        if (level == 0)
        {
            switch (checkpoint)
            {
                case 0:
                    transform.position = new Vector3(54, 2.25f, 24);
                    break;
                case 1:
                    transform.position = new Vector3(0, 1, 8);
                    break;
                case 2:
                    transform.position = new Vector3(11, 1, -33);
                    break;
            }
        }

        GameObject.Find("PauseMenuHolder").GetComponent<PauseMenu>().ResumeGame();

        if(File.Exists(Application.persistentDataPath + "/save.txt"))
        {
            File.WriteAllText(Application.persistentDataPath + "/save.txt", "false");
        }

    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
