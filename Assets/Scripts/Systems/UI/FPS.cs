using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    public TMP_Text text_fps;

    private int frameCounter = 0;
    private float refreshTime = 0.1f;
    private float timeCounter = 0.0f;
    

    void Update()
    {
        if(timeCounter < refreshTime)
        {
            timeCounter += Time.deltaTime;
            frameCounter++;
        } 
        else
        {
            float lastFrameRate = frameCounter / timeCounter;
            frameCounter = 0;
            timeCounter = 0.0f;
            int fps = (int) Math.Floor(Math.Abs(lastFrameRate));
            text_fps.text = fps.ToString() + " FPS";
        }
    }
}
