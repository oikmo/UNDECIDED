using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SettingsData
{
    public int qualityIndex, resolutionIndex, width, height;
    public bool isDaFullScreen, isVSync;
    public float mVolume, sVolume, vVolume, sens, fov;

    public SettingsData(OptionsMenu oMenu)
    {
        fov = oMenu.fov;

        qualityIndex = oMenu.qualityIndex;
        resolutionIndex = oMenu.resolutionIndex;
        
        isDaFullScreen = oMenu.isFullScreen;
        isVSync = oMenu.isVSync;

        width = oMenu.width;
        height = oMenu.height;

        mVolume = oMenu.mVolume;
        sVolume = oMenu.sVolume;
        vVolume = oMenu.vVolume;
        sens = oMenu.sens;
    }
}
