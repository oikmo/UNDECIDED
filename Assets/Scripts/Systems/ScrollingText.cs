using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    [SerializeField] AudioSource sfx;
    [SerializeField] TMP_Text text;
    [SerializeField] float textSpeed;

    public void Awake()
    {
        text.text = string.Empty;
    }

    public void DisplayText(string part)
    {
        StartCoroutine(TypeLine(part));
    }

    IEnumerator TypeLine(string bruh)
    {
        text.text = string.Empty;
        // type char 1 by 1
        foreach (char c in bruh.ToCharArray())
        {
            if(sfx != null) { sfx.Play(); }
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
            if(sfx != null) { sfx.Stop(); }
        }
    }
}
