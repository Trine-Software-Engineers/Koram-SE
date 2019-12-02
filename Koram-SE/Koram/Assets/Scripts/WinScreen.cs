using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinScreen : MonoBehaviour
{
    public static bool Win = false;
    public GameObject WinMenuUI;
    public TextMeshProUGUI ScoreText;
    public GameObject HudUI;

    void Update()
    {
        if (Win) ShowScore();
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

      public void NextLevel()
   {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game... works outside of editor");
        Application.Quit();
    }
}
