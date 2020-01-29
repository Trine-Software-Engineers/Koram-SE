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

        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        Audio.Volume("MenuTheme", SaveManager.GetVolume());
   }

   //Music is controlled from level to level with this function
   public void PlayGame()
   {    
       Debug.Log("started");
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        if(SaveManager.GetLevelsCompleted() + 1 > 19) SceneManager.LoadScene(20);
        else SceneManager.LoadScene(SaveManager.GetLevelsCompleted() + 1);  
        Debug.Log("done");
        
        Audio.Stop("MenuTheme");

        
        if(SaveManager.GetLevelsCompleted() + 1 > 19) Audio.Play("Level20");
        else Audio.Play("Level" + (SaveManager.GetLevelsCompleted() + 1));
   }

    //for quitting the application
   public void QuitGame()
   {
       Debug.Log("Quit");
       Application.Quit();
   } 
}
