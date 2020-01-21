using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SaveMenu : MonoBehaviour
{
    public static int LevelsCompleted = 0;
    public TextMeshProUGUI LevelsCompletedUI;

    void Awake()
    {
        
    }

    //This connects the volume in the settings menu to the actual volume of the sounds 
    void Update()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        LevelsCompletedUI.text = "Levels Completed: " + SaveManager.GetLevelsCompleted();
    }

    public void Save()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        SaveManager.Save();
    }

    public void Load()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        SaveManager.Load();
    }

    public void Delete()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        SaveManager.Delete();
    }
}
