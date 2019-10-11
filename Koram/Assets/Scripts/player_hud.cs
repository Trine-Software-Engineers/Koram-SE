using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//We don't have to have a time limit,
//it can easily be changed to be a hidden way to calculate score by how fast you complete a level

public class player_hud : MonoBehaviour
{
    public float TimeLimit = 120;
    public int PlayerHealth = 100;

    public GameObject TimeLeftUI;
    public Slider HealthBarUI;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //every second, count down and then update the UI.
        TimeLimit -= Time.deltaTime;
        TimeLeftUI.gameObject.GetComponent<Text>().text = ("Time Left: " + (int)TimeLimit);

        //if time reaches 0, reload level (restart level). temporary until we get a proper game over
        if(TimeLimit < 0.1f) SceneManager.LoadScene("Koram");

        //Health Bar
        HealthBarUI.value = PlayerHealth;
    }
}
