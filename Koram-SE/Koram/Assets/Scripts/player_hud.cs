using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player_hud : MonoBehaviour
{
    public int CurrentHealth = 3; // only used in inspector DO NOT USE IN CODE
    public int MaxHealth = 3;  // supports up to 10 hearts
    public static float TimeTaken = 0;

    public static int PlayerHealth = 3; // actual player health
    public Image[] hearts;
    public Sprite HeartFull;
    public Sprite HeartEmpty;
    public GameObject TimeTakenUI;

    void Start()
    {
        BlueGem = GameObject.Find("GemBlue");
        BlackGem = GameObject.Find("GemBlack");
        GreenGem = GameObject.Find("GemGreen");
        RedGem = GameObject.Find("GemRed");
        OrangeGem = GameObject.Find("GemOrange");
        PurpleGem = GameObject.Find("GemPurple");
        BlackGemPrefab = GameObject.Find("BlackGem");
        BlueGemPrefab = GameObject.Find("BlueGem");
        GreenGemPrefab = GameObject.Find("GreenGem");
        RedGemPrefab = GameObject.Find("RedGem");
        OrangeGemPrefab = GameObject.Find("OrangeGem");
        PurpleGemPrefab = GameObject.Find("PurpleGem");

        hearts[0] = GameObject.Find("Heart").GetComponent<Image>(); 
        hearts[1] = GameObject.Find("Heart (1)").GetComponent<Image>(); 
        hearts[2] = GameObject.Find("Heart (2)").GetComponent<Image>(); 
        hearts[3] = GameObject.Find("Heart (3)").GetComponent<Image>(); 
        hearts[4] = GameObject.Find("Heart (4)").GetComponent<Image>(); 
        hearts[5] = GameObject.Find("Heart (5)").GetComponent<Image>(); 
        hearts[6] = GameObject.Find("Heart (6)").GetComponent<Image>(); 
        hearts[7] = GameObject.Find("Heart (7)").GetComponent<Image>(); 
        hearts[8] = GameObject.Find("Heart (8)").GetComponent<Image>(); 
        hearts[9] = GameObject.Find("Heart (9)").GetComponent<Image>(); 

        TimeTakenUI = GameObject.Find("TimeTaken");
    }

    // Update is called once per frame
    void Update()
    {
        //update damage taken
        if(PlayerHealth != CurrentHealth) CurrentHealth = PlayerHealth;

        //update inspector health change
        if(PlayerHealth != CurrentHealth) PlayerHealth = CurrentHealth;


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

        if(gameObject != null)
        {
            //every second, count up and then update the UI.  
            TimeTaken += Time.deltaTime;
            TimeTakenUI.gameObject.GetComponent<Text>().text = ("" + (int)TimeTaken);
        }
        
    }

}
