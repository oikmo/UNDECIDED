using System.Collections;
using UnityEngine;
using TMPro;

public class Cicero : MonoBehaviour
{
    public static Cicero Instance { get; private set; }
    [SerializeField] AudioSource sfx;
    [SerializeField] TMP_Text text;
    [SerializeField] float textSpeed;
    [SerializeField] GameObject gO;

    void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void DisplayText(string part)
    {
        StartCoroutine(TypeLine(part));
    }

    IEnumerator TypeLine(string bruh)
    {
        gO.SetActive(true);
        text.text = string.Empty;
        // type char 1 by 1
        foreach (char c in bruh.ToCharArray())
        {
            sfx.Play();
            text.text += c;
            yield return new WaitForSeconds(textSpeed);
            sfx.Stop();
        }
        sfx.Stop();
        yield return new WaitForSeconds(1);
        text.text = string.Empty;
        yield return new WaitForSeconds(1);
        gO.SetActive(false);
    }
}
