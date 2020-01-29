using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData : MonoBehaviour
{
    //interactable variables
    private int[] currentScore;
    public int[] bestScore;
    private int finalcurrent;
    public int finalOverallScore;
    private int timescore;
    public int levelsCompleted = 0;
    private float volume;

    public bool TouchScreenMode;

    //stored variables for best score
    private int b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15, b16, b17, b18, b19, b20; 

    public static SaveData instance;

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
        Load();
        Audio.Volume("MenuTheme", volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save()
    {
        for(int x = 1; x < 21; x++)
        {
            PlayerPrefs.SetInt("b" + x + "Save", bestScore[x]);
        }
        PlayerPrefs.SetInt("LevelsCompletedSave", levelsCompleted);
        PlayerPrefs.SetInt("finalOverallScoreSave", finalOverallScore);
        PlayerPrefs.SetFloat("volumeSave", volume);

        int temp;
        if(TouchScreenMode) temp = 1;
        else temp = 0; 
        PlayerPrefs.SetInt("TouchScreenModeSave", temp);
    }

    public void Load()
    {
       for(int x = 1; x < 21; x++)
        {
            bestScore[x] = PlayerPrefs.GetInt("b" + x + "Save", 0);
        }
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompletedSave", 0);
        finalOverallScore = PlayerPrefs.GetInt("finalOverallScoreSave", 0);
        volume = PlayerPrefs.GetFloat("volumeSave", .1f);

        int temp = PlayerPrefs.GetInt("TouchScreenModeSave", 1);
        if(temp == 1) TouchScreenMode = true;
        else TouchScreenMode = false;
    }

    public bool GetTouchScreenMode()
    {
        return TouchScreenMode;
    }

    public void SetTouchScreenMode(bool touchScreenUpdated)
    {
        TouchScreenMode = touchScreenUpdated;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void SetVolume(float volumeincoming)
    {
        volume = volumeincoming;
    }

    public void Delete()
    {
        levelsCompleted = 0;
        for (int x = 1; x < 21; x++)
        {
            bestScore[x] = 0;
            timescore = 0;
            finalOverallScore = 0;
        }
        Save();
        Load();
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

    public int GetFinalOverallScore()
    {
        CalculateFinalOverallScore();
        return finalOverallScore;
    }

    private void CalculateFinalOverallScore()
    {
        finalOverallScore = 0;
        for(int x = 1; x < 21; x++)
        {
            finalOverallScore += bestScore[x];
        }
    }

    private void CheckForNewBestScore(int level)
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
