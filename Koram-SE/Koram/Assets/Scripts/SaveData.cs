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
    }

    public void Load()
    {
       for(int x = 1; x < 21; x++)
        {
            bestScore[x] = PlayerPrefs.GetInt("b" + x + "Save", 0);
        }
        levelsCompleted = PlayerPrefs.GetInt("LevelsCompletedSave", 0);
        finalOverallScore = PlayerPrefs.GetInt("finalOverallScoreSave", 0);
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
