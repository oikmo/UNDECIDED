using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    
    public static DebugController Instance { get; private set; }

    public static DebugCommand HELP;
    public static DebugCommand STOP;
    public static DebugCommand<int> HEALTH;
    public static DebugCommand<string, int, int, int> TELEPORT;
    public static DebugCommand<string> GOTO;
    public static DebugCommand<string, int> STAMINA;

    public List<DebugCommandBase> commandList;

    [SerializeField] GameObject Panel;
    [SerializeField] TMP_InputField input;
    [SerializeField] TMP_Text textBody;

    public void OnToggleDebug() 
    {
        showConsole = !showConsole;
        if(showConsole)
        {
            if(!GameHandler.Instance.pMenu.isPaused)
            {
                GameHandler.Instance.pMenu.PauseGame();
            }

            if(File.Exists(Application.persistentDataPath + "/console-save.consave"))
            {
                textBody.text = File.ReadAllText(Application.persistentDataPath + "/console-save.consave");
            }
        }
        
    }

    public void OnReturn()
    {
        if(showConsole)
        {
            if(!string.IsNullOrEmpty(input.text))
            {
                textBody.text += input.text + "\n";
            }
            HandleInput();
            if(input.text != "")
            {   
                GameHandler.Instance.HECHEATED = true;
            }
            input.text = "";
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            DontDestroyOnLoad(Instance);
        }

        HELP = new DebugCommand("help", "Shows a list of commands", "help", () =>
        {
            for(int i=0; i<commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;

                string label = $"{command.commandFormat} - {command.commandDesc}\n";
                textBody.text += label;
            }
        });

        STOP = new DebugCommand("stop", "Stops the game.", "stop", () =>
        {
            textBody.text += "Stopping game.\n";
            File.WriteAllText(Application.persistentDataPath + "/console-save.consave", textBody.text);
            Application.Quit();
            
        });

        HEALTH = new DebugCommand<int>("setHealth", "sets the health of player", "setHealth <int>", (x) =>
        {
            if (GameObject.Find("Player") != null)
            {
                GameObject.Find("Player").GetComponent<PlayerHealth>().curhealth = x;
                textBody.text += "Player Health is now set to : " + x + "\n";
            }
        });

        TELEPORT = new DebugCommand<string, int, int, int>("tp", "teleports the player", "tp <string, int int int>", (t, x, y, z) =>
        {
            Vector3 playerPos = GameObject.Find("Player").transform.position;

            if(GameObject.Find("Player") != null)
            {
                switch(t) {
                    case "add":
                        playerPos += new Vector3(x, y, z);
                        textBody.text += "Setting player to : " + (int)playerPos.x + ", " + (int)playerPos.y + ", " + (int)playerPos.z + "\n";
                        break;
                    case "set":
                        playerPos = new Vector3(x, y, z);
                        textBody.text += "Teleporting player to : " + x + ", " + y + ", " + z + "\n";
                        break;
                }
            }
            GameObject.Find("Player").transform.position = playerPos;
        });

        STAMINA = new DebugCommand<string, int>("setStamina", "sets the stamina of player", "setStamina <string, int>", (t, x) =>
        {
            if (GameObject.Find("Player") != null)
            {
                switch(t) {
                    case "sprint":
                        GameObject.Find("Player").GetComponent<PlayerMovementAdvanced>().sprintingDecrease = x;
                        textBody.text += "sprintingDecrease is now " + x + "\n"; 
                        break;
                    case "climbing":
                        GameObject.Find("Player").GetComponent<PlayerMovementAdvanced>().climbingDecrease = x;
                        textBody.text += "climbingDecrease is now " + x + "\n"; 
                        break;
                    case "wallrunning":
                        GameObject.Find("Player").GetComponent<PlayerMovementAdvanced>().wallrunningDecrease = x;
                        textBody.text += "wallrunningDecrease is now " + x + "\n"; 
                        break;
                }
            }
        });

        GOTO = new DebugCommand<string>("goto", "go to scene", "goto <string>", (x) =>
        {
            textBody.text += "Now loading : " + x + "\n";
            File.WriteAllText(Application.persistentDataPath + "/console-save.consave", textBody.text);
            GameObject.Find("LevelLoader").GetComponent<LevelLoader>().LoadLevel(x);
        });

        commandList = new List<DebugCommandBase>
        {
            HELP,
            STOP,
            TELEPORT,
            HEALTH,
            STAMINA,
            GOTO
        };
    }

    public void Print(string text)
    {
        textBody.text += text + "\n";
    }
    public void PrintInfo(string text)
    {
        textBody.text += "[INFO] " + text + "\n";
    }
    public void PrintWarning(string text)
    {
        textBody.text += "[WARNING] " + text + "\n";
    }
    public void PrintError(string text)
    {
        textBody.text += "[ERROR] " + text + "\n";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            OnToggleDebug();
        }

        if(Panel.activeSelf != showConsole)
        {
            Panel.SetActive(showConsole);
        }

        if(showConsole && !GameHandler.Instance.pMenu.isPaused)
        {
            showConsole = false;
        }

    }

    private void HandleInput()
    {
        string[] properties = input.text.Split(' ');

        if(commandList != null)
        {
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase commandBase = commandList[i];

                if (input.text.Contains(commandBase.commandID))
                {
                    if (commandList[i] as DebugCommand != null)
                    {
                        (commandList[i] as DebugCommand).Invoke();
                    }
                    else if (commandList[i] as DebugCommand<int> != null)
                    {
                        (commandList[i] as DebugCommand<int>).Invoke(int.Parse(properties[1]));
                    }
                    else if (commandList[i] as DebugCommand<string, string> != null)
                    {
                        if (commandBase.commandID.Contains("goto"))
                        {
                            if (GameObject.Find("Player") != null)
                            {
                                try
                                {
                                    (commandList[i] as DebugCommand<string, string>).Invoke(properties[1], properties[2]);
                                }
                                catch (FormatException) { }
                            }
                        }
                    }
                    else if (commandList[i] as DebugCommand<string, int, int, int> != null)
                    {
                        if (commandBase.commandID.Contains("tp"))
                        {
                            if (GameObject.Find("Player") != null)
                            {
                                try
                                {
                                    (commandList[i] as DebugCommand<string, int, int, int>).Invoke(properties[1], int.Parse(properties[2]), int.Parse(properties[3]), int.Parse(properties[4]));
                                }
                                catch (FormatException) { }
                            }
                        }
                    }

                    else if (commandList[i] as DebugCommand<string, int> != null)
                    {
                        if (commandBase.commandID.Contains("setStamina"))
                        {
                            if (GameObject.Find("Player") != null)
                            {
                                try
                                {
                                    (commandList[i] as DebugCommand<string, int>).Invoke(properties[1], int.Parse(properties[2]));
                                }
                                catch (FormatException) { }
                            }
                        }
                    }

                    else if (commandList[i] as DebugCommand<string> != null)
                    {
                        if(commandBase.commandID.Contains("goto"))
                        {
                            if(GameObject.Find("LevelLoader"))
                            {
                                (commandList[i] as DebugCommand<string>).Invoke(properties[1]);
                            }
                        }
                    }
                }

            }
        }
        
        File.WriteAllText(Application.persistentDataPath + "/console-save.consave", textBody.text);
    }

    public void OnApplicationQuit()
    {
        File.WriteAllText(Application.persistentDataPath + "/console-save.consave", "");
    }
}
