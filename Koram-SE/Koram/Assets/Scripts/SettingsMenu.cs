using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Toggle ToggleUI;
    public Slider VolumeBarUI;

    void Awake()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        VolumeBarUI.value = SaveManager.GetVolume();
        ToggleUI.isOn = SaveManager.GetTouchScreenMode();
    }

    //This connects the volume in the settings menu to the actual volume of the sounds 
    void Update()
   {
       SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
       SaveManager.SetVolume(VolumeBarUI.value);  

       Audio.Volume("MenuTheme", SaveManager.GetVolume());

       if(ToggleUI.isOn == true) SaveManager.SetTouchScreenMode(true);
       else SaveManager.SetTouchScreenMode(false);
   }
}
