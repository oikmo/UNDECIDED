using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text_sens, text_fov;
    public GameObject gO, backButton;
    [SerializeField] AudioMixer musicMixer, sfxMixer, voiceMixer;
    [SerializeField] Slider sensSlide, mVolumeSlider, sVolumeSlider, vVolumeSlider, fovS;
    [SerializeField] Toggle fScreen, vsync;
    [SerializeField] TMP_Dropdown quality, resolution;

    public int width = Screen.width, height = Screen.height;
    public int qualityIndex = 0, resolutionIndex = 3;
    public bool isOptions, isFullScreen = true, isVSync= false;
    public float mVolume = -20f, sVolume = -10f, vVolume = -10f, fov = 90f, sens = 50f;
    private Guid Latest;
    bool hasSet, hasSetFOV;

    void Start()
    {
        sens = sensSlide.value;
        if (LoadSettings())
        {
            
            fovS.value = fov;
            mVolumeSlider.value = mVolume;
            sVolumeSlider.value = sVolume;
            vVolumeSlider.value = vVolume;
            quality.value = qualityIndex;
            resolution.value = resolutionIndex;
            sensSlide.value = sens;
            fScreen.isOn = isFullScreen;
            vsync.isOn = isVSync;
        }
        else
        {
            sens = 50;
            fov = 90;
            mVolume = -20;
            sVolume = -10;
            vVolume = -10;
            qualityIndex = 0;
            resolutionIndex = 3;
            width = Screen.width;
            height = Screen.height;
            isFullScreen = false;
            isVSync = false;
            saveSettings();
            sensSlide.value = sens;
            fovS.value = fov;
            mVolumeSlider.value = mVolume;
            sVolumeSlider.value = sVolume;
            vVolumeSlider.value = vVolume;
            quality.value = qualityIndex;
            sensSlide.value = sens;
            fScreen.isOn = isFullScreen;
            vsync.isOn = isVSync;
        }

        if(GameObject.Find("PlayerCam") != null)
        {
            if(GameObject.Find("PlayerCam").GetComponent<PlayerCam>() != null)
            {
                if(fov != 0)
                {
                    GameObject.Find("PlayerCam").GetComponent<PlayerCam>().cam.fieldOfView = fov;
                }
                else
                {
                    GameObject.Find("PlayerCam").GetComponent<PlayerCam>().cam.fieldOfView = 90;
                }
            }
        }
    }

    void Update()
    {

        //Debug.Log(hasSet);
        if (gO != null)
        {
            if (gO.activeSelf)
            {
                isOptions = true;

                if (!hasSet)
                {
                    EventSystem.current.SetSelectedGameObject(backButton);
                    hasSet = true;
                }
            }
            else
            {
                isOptions = false;
                hasSet = false;
            }
        }

        musicMixer.SetFloat("volume", mVolume); //mVolume
        sfxMixer.SetFloat("volume", sVolume); //sVolume
        voiceMixer.SetFloat("volume", vVolume); //vVolume

        if(GameObject.Find("PlayerCam") != null)
        {
            if (GameObject.Find("PlayerCam").GetComponent<PlayerCam>() != null)
            {
                GameObject.Find("PlayerCam").GetComponent<PlayerCam>().sensX = sens; //sensX
                GameObject.Find("PlayerCam").GetComponent<PlayerCam>().sensY = sens; //sensY

                if(!hasSetFOV)
                {
                    GameObject.Find("PlayerCam").GetComponent<PlayerCam>().cam.fieldOfView = fov;
                    hasSetFOV = true;
                }
            }
        }

        text_sens.text = sens.ToString();
        text_fov.text = fov.ToString();

        if(width != int.Parse(resolution.options[resolution.value].text.Split('x')[0]))
        {
            width = int.Parse(resolution.options[resolution.value].text.Split('x')[0]);
            Screen.SetResolution(width, height, isFullScreen);
            saveSettings();
            loadSettings();
        }

        if (height != int.Parse(resolution.options[resolution.value].text.Split('x')[1]))
        {
            height = int.Parse(resolution.options[resolution.value].text.Split('x')[1]);
            Screen.SetResolution(width, height, isFullScreen);
            saveSettings();
            loadSettings();
        }
        

        if (text_sens.text == "1") { text_sens.text = "PAIN"; }
        if (text_sens.text == "100") { text_sens.text = "WHY"; }

        if (text_fov.text == "1") { text_fov.text = "ZOOM"; }
        if (text_fov.text == "120") { text_fov.text = "QUAKE PRO"; }

        Screen.fullScreen = isFullScreen; //isDaFullScreen

        if(QualitySettings.GetQualityLevel() != qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        if (isVSync) { QualitySettings.vSyncCount = 1; } //isVSync
        else { QualitySettings.vSyncCount = 0; }

        qualityIndex = quality.value;
        quality.onValueChanged.AddListener(delegate { StartCoroutine(Debounced(1)); });
    }


    public void OpenPauseMenu()
    {
        if(GameHandler.Instance != null)
        {
            if(GameHandler.Instance.oMenu != null)
            {
                GameHandler.Instance.pMenu.pMenu.SetActive(true);
                GameHandler.Instance.pMenu.isPaused = true;

                isOptions = false;
                gO.SetActive(false);
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        mVolume = volume;
        saveSettings();
        loadSettings();

    }

    public void SetSFXVolume(float volume)
    {
        sVolume = volume;
        saveSettings();
        loadSettings();
    }

    public void SetVoiceVolume(float volume)
    {
        vVolume = volume;
        saveSettings();
        loadSettings();
    }

    public void setSens(float sens)
    {
        this.sens = sens;
        saveSettings();
        loadSettings();
    }

    public void setFOV(float fov)
    {
        this.fov = fov;
        hasSetFOV = false;
        saveSettings();
        loadSettings();
    }

    public void SetFullScreen(bool isFullScreen)
    {
        this.isFullScreen = isFullScreen;
        saveSettings();
        loadSettings();
    }

    public void SetVSync(bool isVSync)
    {
        this.isVSync = isVSync;
        saveSettings();
        loadSettings();
    }

    public void SetResolution(int index)
    {
        resolutionIndex = index;
        Screen.SetResolution(width, height, isFullScreen);
        saveSettings();
        loadSettings();
    }

    public void saveSettings()
    {
        SaveSystem.SaveSettings(this);
    }

    public void loadSettings()
    {
        SettingsData data = SaveSystem.LoadSettings();

        if (data != null)
        {
            fov = data.fov;

            width = data.width;
            height = data.height;
            qualityIndex = data.qualityIndex;
            resolutionIndex = data.resolutionIndex;

            isFullScreen = data.isDaFullScreen;
            isVSync = data.isVSync;

            mVolume = data.mVolume;
            sVolume = data.sVolume;
            vVolume = data.vVolume;
            sens = data.sens;
        }
    }

    public bool LoadSettings()
    {
        bool isCheck = false;
        SettingsData data = SaveSystem.LoadSettings();

        if (data != null)
        {
            fov = data.fov;

            width = data.width;
            height = data.height;
            qualityIndex = data.qualityIndex;
            resolutionIndex = data.resolutionIndex;

            isFullScreen = data.isDaFullScreen;
            isVSync = data.isVSync;

            mVolume = data.mVolume;
            sVolume = data.sVolume;
            vVolume = data.vVolume;
            sens = data.sens;

            isCheck = true;
        }

        return isCheck;
        
    }

    private IEnumerator Debounced(int s)
    {
        var guid = Guid.NewGuid();
        Latest = guid;

        yield return new WaitForSeconds(s);

        if (Latest == guid)
        {
            saveSettings();
            //Debounced(1);
            loadSettings();
        }
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
