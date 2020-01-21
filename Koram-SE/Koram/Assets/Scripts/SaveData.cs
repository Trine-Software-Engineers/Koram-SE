using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData : MonoBehaviour
{

    public static SaveData instance;
    public int levelsCompleted = 0;

    void Awake()
    {
        if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        PlayerPrefs.SetInt("LevelsCompletedSave", levelsCompleted);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("LevelsCompletedSave"))
        {
            //if there is a save, load it. Default is 0 levels completed
            levelsCompleted = PlayerPrefs.GetInt("LevelsCompletedSave", 0);
        }
        else Debug.Log("No save available.");
    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        Write(0);
        Debug.Log("Levels Completed Save Deleted");
    }

    public void Write(int updatedLevelsCompleted)
    {
        levelsCompleted = updatedLevelsCompleted;   
    }

    public int GetLevelsCompleted()
    {
        return levelsCompleted;
    }

}
