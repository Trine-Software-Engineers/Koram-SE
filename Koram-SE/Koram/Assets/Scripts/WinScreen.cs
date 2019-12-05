using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public static bool Win = false;
    public GameObject WinMenuUI;
    public GameObject FinalMenuUI;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI FinalScoreText;
    public GameObject HudUI;
    public static bool Final = false;

    void Update()
    {
        if (Win) ShowScore();
        if (Final) ShowFinal();
        
    }

    void ShowScore()
    {
        Time.timeScale = 0f;
        WinMenuUI.SetActive(true);

        int score = (int)(100 - player_hud.TimeTaken);
        if (score < 0) score = 0;
        string finalscore = score.ToString();
        ScoreText.text = ("Score: " + finalscore);
        
        HudUI.SetActive(false);
    }
    
    void ShowFinal()
    {
        
        Time.timeScale = 0f;
        FinalMenuUI.SetActive(true);

        int score = (int)(100 - player_hud.TimeTaken);
        if (score < 0) score = 0;
        string finalscore = score.ToString();
        FinalScoreText.text = ("Final Score: " + finalscore);
        
        HudUI.SetActive(false);
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
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Audio.Stop("Level" + (SceneManager.GetActiveScene().buildIndex).ToString());
        Win = false;
        WinMenuUI.SetActive(false);

        SceneManager.LoadScene("Main");

    }

    public void QuitGame()
    {
        Debug.Log("Quit Game... works outside of editor");
        Application.Quit();
    }
}
