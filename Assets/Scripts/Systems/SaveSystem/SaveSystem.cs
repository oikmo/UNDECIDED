using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
using TMPro;

public static class SaveSystem
{
    public static string saveName;
    public static void SavePlayer()
    {
        BinaryFormatter format = new BinaryFormatter();
        
        string path = Path.Combine(Application.persistentDataPath + "/Saves/" + saveName + ".mds");
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData();

        format.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath + "/Saves/" + saveName + ".mds");

        if(File.Exists(path))
        {
            BinaryFormatter format = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = format.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("save not found in " + path);
            return null;
        }
    }

    public static void SaveSettings(OptionsMenu oMenu)
    {
        BinaryFormatter format = new BinaryFormatter();

        string path = Path.Combine(Application.persistentDataPath + "/config.prefs");
        FileStream stream = new FileStream(path, FileMode.Create);

        SettingsData data = new SettingsData(oMenu);

        format.Serialize(stream, data);
        stream.Close();
    }

    public static SettingsData LoadSettings()
    {
        string path = Path.Combine(Application.persistentDataPath + "/config.prefs");

        if (File.Exists(path))
        {
            BinaryFormatter format = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SettingsData data = format.Deserialize(stream) as SettingsData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("save not found in " + path);
            return null;
        }
    }

    public static void setName(string name)
    {
        saveName = name;
    }
}
