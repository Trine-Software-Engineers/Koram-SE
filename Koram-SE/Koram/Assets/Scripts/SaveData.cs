using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData : MonoBehaviour
{
    
    public int[] currentScore;
    public int[] bestScore;

    public int finalcurrent;
    public int finalmax;
    
    public int timescore;

    public static SaveData instance;
    public int levelsCompleted = 0;

    void Awake()
    {
        currentScore = new int[21];
        bestScore = new int[21];

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


    public int GetBestScore(int level)
    {
        return bestScore[level];
    }

    public void CheckForNewBestScore(int level)
    {
        if(currentScore[level] > bestScore[level]) bestScore[level] = currentScore[level];
    }

    public void UpdateCurrentScore(int level, int points)
    {
        currentScore[level] += points;
    } 

    public int GetCurrentScore(int level)
    {
        return currentScore[level];
    }

    public int GetFinalScoreForLevel(int level)
    {
        timescore = 0;
        timescore += (int)(1000 - player_hud.TimeTaken);
        if(timescore > 0) currentScore[level] += timescore;

        int temp = currentScore[level];
        CheckForNewBestScore(level);
        currentScore[level] = 0;
        return temp;
    }

    public void ClearCurrentScore(int level)
    {
        currentScore[level] = 0;
    }

}
