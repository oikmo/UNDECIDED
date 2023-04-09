using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Net;
using System;

public class Facts : MonoBehaviour
{
    bool connected = true;

    [SerializeField] TMP_Text text;
    string[] lines;
    void Start()
    {
        string raw = "";
        try
        {
            WebClient client = new WebClient();
            raw = client.DownloadString("https://chappie-webpages.werdimduly.repl.co/facts/");
        } 
        catch (WebException) 
        {
            text.text = "[couldn't connect to internet]";
            connected = false;
        }
        if(connected)
        {
            lines = raw.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                line.TrimEnd('\n');
            }
            InvokeRepeating("changeText", 0.0f, 4);
        }
        
    }

    void changeText()
    {
        if (lines != null && lines.Length != 0 && connected)
        {
            System.Random ran = new System.Random();
            text.text = lines[ran.Next(0, lines.Length)];
        }
    }
}
