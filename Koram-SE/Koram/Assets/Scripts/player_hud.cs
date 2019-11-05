using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class player_hud : MonoBehaviour
{
    public float TimeTaken = 0;
    public GameObject TimeTakenUI;

    public int CurrentHealth = 3; // only used in inspector DO NOT USE IN CODE
    public int MaxHealth = 3;  // supports up to 10 hearts
    public static int PlayerHealth = 3; // actual player health
    public Image[] hearts;
    public Sprite HeartFull;
    public Sprite HeartEmpty;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerHealth != CurrentHealth) 
        {
            PlayerHealth = CurrentHealth;
        }

        if(PlayerHealth > MaxHealth) 
        {
            PlayerHealth = MaxHealth;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < PlayerHealth)
            {
                hearts[i].sprite = HeartFull;
            } 
            else 
            {
                hearts[i].sprite = HeartEmpty;
            }


            if(i < MaxHealth)
            {
                hearts[i].enabled = true;
            } 
            else 
            {
                hearts[i].enabled = false;
            }
        }

        //every second, count up and then update the UI.  
        TimeTaken += Time.deltaTime;
        TimeTakenUI.gameObject.GetComponent<Text>().text = ("" + (int)TimeTaken);

    }

}
