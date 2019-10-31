using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public float Volume = 0.1f;
    public Slider VolumeBarUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
   {
       Volume = VolumeBarUI.value;
       Debug.Log(Volume);
       
       Audio.Volume("MenuTheme", Volume);
   }
}
