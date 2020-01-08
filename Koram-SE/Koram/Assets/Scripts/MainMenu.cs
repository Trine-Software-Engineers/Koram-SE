using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

   //Starts the main menu music when the game is loaded
   void Start()
   {
       Audio.Play("MenuTheme");
       Audio.Volume("MenuTheme", 0.1f);
   }

   //Music is controlled from level to level with this function
   public void PlayGame()
   {    
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
        Audio.Stop("MenuTheme");
        Audio.Play("Level" + (SceneManager.GetActiveScene().buildIndex + 1).ToString());
   }

    //for quitting the application
   public void QuitGame()
   {
       Debug.Log("Quit");
       Application.Quit();
   } 
}
