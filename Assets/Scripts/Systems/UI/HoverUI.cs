using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class HoverUI : MonoBehaviour
{
    public static ItemObject tempItem;
    public static bool isFollow;
    [Header("UI")]
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text descText;
    [SerializeField] GameObject parent;

    void Update()
    {
        if (tempItem != null)
        {
            itemNameText.text = tempItem.itemName;
            descText.text = tempItem.description;
        }
        else
        {
            itemNameText.text = "<name here>";
            descText.text = "<desc here>";
        }
        

        if(isFollow || tempItem != null)
        {
            GetComponent<CanvasGroup>().alpha = 1f;
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 0f;
        }


        GameHandler.ClampToWindow(Input.mousePosition, GetComponent<RectTransform>(), parent.GetComponent<RectTransform>());
    }

    
}