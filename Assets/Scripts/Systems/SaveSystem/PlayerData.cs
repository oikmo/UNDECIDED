using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int version;
    public int checkpoint;
    public int level;
    public int health;
    public float moveSpeed;
    public bool HECHEATED;

    public PlayerData()
    {
        if(GameHandler.Instance != null)
        {
            if(GameHandler.Instance.pStuff)
            {
                PlayerStuff pStuff = GameHandler.Instance.pStuff;
                level = pStuff.level;
                health = (int)GameHandler.Instance.pHealth.curhealth; 
                checkpoint = pStuff.checkpoint;
                HECHEATED = GameHandler.Instance.HECHEATED;
                version = pStuff.version; 
            }
        } 
    }
}
