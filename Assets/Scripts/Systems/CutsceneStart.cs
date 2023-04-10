using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(SignalReceiver))]
public class CutsceneStart : MonoBehaviour
{

    [SerializeField] GameObject cutsceneToPlay;

    public void Activate() 
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameHandler.Instance.cursor.SetActive(false);
        GameHandler.Instance.cam.GetComponent<PlayerCam>().renderlayers.SetActive(false);
        GameHandler.Instance.cam.GetComponent<PlayerCam>().enabled = false;
        GameHandler.Instance.cam.enabled = false;

        if(cutsceneToPlay != null) 
        {
            cutsceneToPlay.SetActive(true);
            cutsceneToPlay.GetComponent<PlayableDirector>().Play();
        }
        else if(cutsceneToPlay == null) 
        {
            GetComponent<PlayableDirector>().Play();
        }
        else if(!GetComponent<PlayableDirector>()) 
        {
            Debug.LogWarning("Cutscene could not be loaded!" + gameObject.name);
        }
        
        GameHandler.Instance.cam.GetComponent<PlayerCam>().cutsceneHolder.transform.Find("CutsceneCamera").GetComponent<Camera>().fieldOfView = SaveSystem.LoadSettings().fov;
        GameHandler.Instance.cam.GetComponent<PlayerCam>().cutsceneHolder.transform.Find("CutscenePlayerCamera").GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = SaveSystem.LoadSettings().fov;
        GameHandler.Instance.cam.GetComponent<PlayerCam>().cutsceneHolder.SetActive(true);
        GameHandler.Instance.cutscene = true;
    }

    public void Deactivate() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameHandler.Instance.cursor.SetActive(true);
        GameHandler.Instance.cam.GetComponent<PlayerCam>().renderlayers.SetActive(true);
        GameHandler.Instance.cam.GetComponent<PlayerCam>().enabled = true;
        GameHandler.Instance.cam.enabled = true;

        if(cutsceneToPlay != null) 
        {
            cutsceneToPlay.SetActive(false);
        }
        GameHandler.Instance.cam.GetComponent<PlayerCam>().cutsceneHolder.SetActive(false);
        GameHandler.Instance.cutscene = false;
    }

    public void ActivateWithoutCM()
    {
        if(cutsceneToPlay != null) 
        {
            cutsceneToPlay.SetActive(true);
            cutsceneToPlay.GetComponent<PlayableDirector>().Play();
        }
        else if(cutsceneToPlay == null) 
        {
            GetComponent<PlayableDirector>().Play();
        }
        else if(!GetComponent<PlayableDirector>()) 
        {
            Debug.LogWarning("Cutscene could not be loaded!" + gameObject.name);
        }
        GameHandler.Instance.cutscene = true;
    }

    public void DeactivateWithoutCM()
    {
        if(cutsceneToPlay != null) 
        {
            cutsceneToPlay.SetActive(false);
        }
        GameHandler.Instance.cutscene = false;
    }
    

    public void TurnVisibilityOfUI(bool visible) 
    {
        GameHandler.Instance.canvas.SetActive(visible);
    }
}
