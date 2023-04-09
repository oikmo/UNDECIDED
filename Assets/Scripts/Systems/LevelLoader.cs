using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{

    public GameObject LoadingScreen;
    public Slider slider;
    public Text ProgressText;

    private void Start()
    {
        LoadingScreen.SetActive(false);
    }

    public void LoadLevel(string Scene)
    {
        StartCoroutine(LoadAsynchronously(Scene));
    }

    IEnumerator LoadAsynchronously (string Scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(Scene);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            int prog = (int) Math.Floor(Math.Abs(progress*100));
            slider.value = prog;
            ProgressText.text = prog + "%";
            
            yield return null;
        }

        LoadingScreen.SetActive(false);
    }

}
