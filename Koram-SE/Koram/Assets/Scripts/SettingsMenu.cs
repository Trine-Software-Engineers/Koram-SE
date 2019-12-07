using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public float Volume = 0.1f;
    public Slider VolumeBarUI;

    //This connects the volume in the settings menu to the actual volume of the sounds 
    void Update()
   {
       Volume = VolumeBarUI.value;       
       Audio.Volume("MenuTheme", Volume);
   }
}
