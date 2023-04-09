using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[RequireComponent(typeof(SignalReceiver))]
public class CutsceneStart : MonoBehaviour
{

    [SerializeField] GameObject cutsceneToPlay;
    public GameObject cutsceneCamera;
    public GameObject cutscenePlayerCamera;

    public void Activate() 
    {
        GameHandler.Instance.cursor.SetActive(false);
        cutsceneToPlay.SetActive(true);
        cutsceneToPlay.GetComponent<PlayableDirector>().Play();
        cutsceneCamera.GetComponent<Camera>().fieldOfView = SaveSystem.LoadSettings().fov;
        cutscenePlayerCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = SaveSystem.LoadSettings().fov;
        GameHandler.Instance.cutscene = true;
        
        cutsceneCamera.SetActive(true);
        cutscenePlayerCamera.SetActive(true);
        GameHandler.Instance.cam.gameObject.SetActive(false);
    }

    public void Deactivate() 
    {
        GameHandler.Instance.cursor.SetActive(true);
        GameHandler.Instance.cutscene = false;
        cutsceneToPlay.SetActive(false);
        cutsceneCamera.SetActive(false);
        cutscenePlayerCamera.SetActive(false);
        GameHandler.Instance.cam.gameObject.SetActive(true);
    }
}
