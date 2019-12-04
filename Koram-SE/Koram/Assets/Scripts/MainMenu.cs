using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

   // Start is called before the first frame update
   void Start()
   {
       Audio.Play("MenuTheme");
       Audio.Volume("MenuTheme", 0.1f);
   }

   public void PlayGame()
   {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
        Audio.Stop("MenuTheme");
        Audio.Play("Level" + (SceneManager.GetActiveScene().buildIndex + 1).ToString());
   }

   public void QuitGame()
   {
       Debug.Log("Quit");
       Application.Quit();
   }


  
}
