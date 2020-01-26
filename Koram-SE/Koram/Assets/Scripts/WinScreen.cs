using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class WinScreen : MonoBehaviour
{
    public static bool Win = false;
    //public static int score;
    //private int timescore;
    public GameObject WinMenuUI;
    public GameObject FinalMenuUI;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestScoreText;
    public TextMeshProUGUI FinalScoreText;
    public GameObject HudUI;
    public static bool Final = false;
    public int count = 0;

    private bool showedScore = false;
    private bool showedFinalScore = false;

    
    //chooses which screen to show when a level is completed
    void Update()
    {
        if (Win) ShowScore();
        if (Final) ShowFinal();    
    }

    //display to user the user score that they got in the completed level for all levels except for 10
    void ShowScore()
    {
        if(showedScore) return;
   
        Time.timeScale = 0f;
        WinMenuUI.SetActive(true);
               
        StartCoroutine(ShowScoreAnimated());
        
        HudUI.SetActive(false);

        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        if(SceneManager.GetActiveScene().buildIndex >= SaveManager.GetLevelsCompleted())
        {
            SaveManager.Write(SceneManager.GetActiveScene().buildIndex);
        }
        else Debug.Log("Save manager thinks you just beat replayed a level.");

        SaveManager.Save();
        showedScore = true;
    }

    
    //Final score and screen on level 20
    void ShowFinal()
    {
        if(showedFinalScore) return;

        Time.timeScale = 0f;
        FinalMenuUI.SetActive(true);

        StartCoroutine(ShowFinalScoreAnimated());

        HudUI.SetActive(false);

        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        if(SceneManager.GetActiveScene().buildIndex >= SaveManager.GetLevelsCompleted())
        {
            SaveManager.Write(SceneManager.GetActiveScene().buildIndex);
        }
        else Debug.Log("Save manager thinks you just beat replayed a level.");

        Debug.Log("Ran through showfinal()");

        SaveManager.Save();
        showedFinalScore = true;
    }


    IEnumerator ShowFinalScoreAnimated()
    {
        Debug.Log("display final score");
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        int trash = (SaveManager.GetFinalScoreForLevel(SceneManager.GetActiveScene().buildIndex));
        int targetScore = (SaveManager.GetFinalOverallScore());
        float tempScore = 0;

        while (tempScore < targetScore)
        {           
            tempScore += Time.unscaledDeltaTime * 2000f; 
            tempScore = Mathf.Clamp(tempScore, 0f, targetScore);
            FinalScoreText.text = ("Final Score: " + ((int)tempScore).ToString());
            yield return null;
        }
        SaveManager.ClearCurrentScore(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ShowScoreAnimated()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        int targetScore = (SaveManager.GetFinalScoreForLevel(SceneManager.GetActiveScene().buildIndex));
        float tempScore = 0;

        while (tempScore < targetScore)
        {           
            tempScore += Time.unscaledDeltaTime * 2000f; 
            tempScore = Mathf.Clamp(tempScore, 0f, targetScore);
            ScoreText.text = ("Score: " + ((int)tempScore).ToString());
            yield return null;
        }
        BestScoreText.text = ("Best Score: " + (SaveManager.GetBestScore(SceneManager.GetActiveScene().buildIndex).ToString()));
        SaveManager.ClearCurrentScore(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Win = false;
        WinMenuUI.SetActive(false);
        HudUI.SetActive(true);

        player_hud.TimeTaken = 0;
        Audio.Stop("Level" + (SceneManager.GetActiveScene().buildIndex).ToString());
        Audio.Play("Level" + (SceneManager.GetActiveScene().buildIndex + 1).ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1f;

        showedScore = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Audio.Stop("Level" + (SceneManager.GetActiveScene().buildIndex).ToString());
        Win = false;
        WinMenuUI.SetActive(false);

        SceneManager.LoadScene("Main");

        showedScore = false;
    }

    //quits the game 
    public void QuitGame()
    {
        showedScore = false;
        Debug.Log("Quit Game... works outside of editor");
        Application.Quit();      
    }
}
