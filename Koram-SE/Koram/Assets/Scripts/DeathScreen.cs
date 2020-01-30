using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{

    public static bool GameIsDead = false;

    public GameObject DeathMenuUI;

    void Update()
    {
        if (GameIsDead)
        {
            Pause();
        }
        else
        {
            if (PauseScreen.GameIsPaused == false)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        DeathMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsDead = false;

    }

    public void Pause()
    {
        DeathMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsDead = true;
    }

    public void LoadMenu()
    {
        GameIsDead = false;
        DeathMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Audio.Stop("Level" + (SceneManager.GetActiveScene().buildIndex).ToString());
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game... works outside of editor");
        Application.Quit();
    }
    
}
