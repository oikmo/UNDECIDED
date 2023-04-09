using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    private GameObject dialogueBody;
    private TMP_Text nameBody, mainBody;
    public string[] lines;
    public bool alreadyTalking;

    private int index;

    void Start()
    {
        alreadyTalking = false;
    }

    void Update()
    {
        print("MainBody : " + (mainBody == null) + " NameBody : " + (nameBody == null));

        if(dialogueBody == null) { dialogueBody = GameHandler.Instance.dialogue; }
        else
        {
            if(nameBody == null) { nameBody = dialogueBody.transform.Find("Name").GetComponent<TMP_Text>(); }
            if(mainBody == null) { mainBody = dialogueBody.transform.Find("Text").GetComponent<TMP_Text>(); mainBody.text = ""; }
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return))
        {
            if (mainBody.text == lines[index].Split('|')[1])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                mainBody.text = lines[index].Split('|')[1];
            }
        }

        if(nameBody != null)
        {
            nameBody.text = lines[index].Split('|')[0];
        }
        
    }

    public void StartDialogue()
    {
        alreadyTalking = true;
        index = 0;
        dialogueBody.SetActive(true);
        mainBody.text = "";
        StartCoroutine(TypeLine());
        GameHandler.Instance.mouse = true;
    }

    IEnumerator TypeLine()
    {
        // type char 1 by 1
        foreach (char c in lines[index].Split('|')[1].ToCharArray())
        {
            mainBody.text += c;
            yield return new WaitForSeconds(float.Parse(lines[index].Split('|')[2]));
        }
    }

    void NextLine()
    {
        if(index < lines.Length - 1)
        {
            index++;
            mainBody.text = "";
            nameBody.text = lines[index].Split('|')[0];
            StartCoroutine(TypeLine());
        } 
        else
        {
            GameHandler.Instance.mouse = false;
            alreadyTalking = false;
            dialogueBody.SetActive(false);
            
        }
    }
}
