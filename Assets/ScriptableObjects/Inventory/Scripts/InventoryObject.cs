using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
[System.Serializable]
public class InventoryObject : ScriptableObject//, ISerializationCallbackReceiver
{
    public ItemDatabaseObject database;
    public List<invSlot> Container = new List<invSlot>();
    public bool isError;

    public void AddItem(ItemObject _item, int _amount)
    {
        for (int i = 0; i < Container.Count; i++)
        {
            if(Container[i].item == _item && Container[i].amount < _item.maxStackSize)
            {
                Container[i].AddAmount(_amount);
                return;
            }
        }

        if(_amount != 0)
        {
            Container.Add(new invSlot(database.GetID[_item], _item, _amount));
        } 
        
    }

    public void Save()
    {
        if(!Directory.Exists(Path.Combine(Application.persistentDataPath + "/Saves/inventory/"))) 
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath + "/Saves/inventory/"));
        }

        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Path.Combine(Application.persistentDataPath + "/Saves/inventory/save.inventory")));
        bf.Serialize(file, saveData);
        file.Close();
        Debug.Log(Path.Combine(Application.persistentDataPath + "/Saves/inventory/save.inventory"));
        Debug.Log("saved");
    }

    public void Load()
    {
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath + "/Saves/inventory/")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath + "/Saves/inventory/"));
        }
        try
        {
            if (File.Exists(string.Concat(Application.persistentDataPath, "/Saves/inventory/save.inventory")))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Path.Combine(Application.persistentDataPath + "/Saves/inventory/save.inventory"), FileMode.Open);
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
                file.Close();
            }
        } catch (SerializationException)
        {
            isError = true;
        }
        Debug.Log(Path.Combine(Application.persistentDataPath + "/Saves/inventory/save.inventory"));
        Debug.Log("loaded");
    }

    public void OnAfterDeserialize() 
    {
        for (int i=0; i<Container.Count; i++) 
        { 
            Container[i].item = database.GetItem[Container[i].ID]; 
        } 
    }
    public void OnBeforeSerialize() {}

}

[System.Serializable]
public class invSlot
{
    public int ID;
    public ItemObject item;
    public int amount;

    public invSlot(int _id, ItemObject _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    
    public bool Equals(invSlot other)
    {
        if (this.ID == other.ID && this.item == other.item && this.amount == other.amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}