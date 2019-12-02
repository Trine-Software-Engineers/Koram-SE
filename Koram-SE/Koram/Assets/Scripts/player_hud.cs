using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player_hud : MonoBehaviour
{
    public int CurrentHealth = 3; // only used in inspector DO NOT USE IN CODE
    public static int MaxHealth = 3;  // supports up to 10 hearts
    public static float TimeTaken = 0;

    public static int PlayerHealth = 3; // actual player health
    public Image[] hearts;
    public Sprite HeartFull;
    public Sprite HeartEmpty;
    public GameObject TimeTakenUI;
    public GameObject BlueGem;
    public GameObject GreenGem;
    public GameObject RedGem;
    public GameObject BlackGem;
    public GameObject PurpleGem;
    public GameObject OrangeGem;
    private GameObject BlackGemPrefab;
    private GameObject BlueGemPrefab;
    private GameObject GreenGemPrefab;
    private GameObject RedGemPrefab;
    private GameObject OrangeGemPrefab;
    private GameObject PurpleGemPrefab;

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
            if(TimeTakenUI != null) TimeTakenUI.gameObject.GetComponent<Text>().text = ("" + (int)TimeTaken);
        }

        GemCheck();
        
    }
    void GemCheck()
    {
        if (BlueGemPrefab == null)
        {
            BlueGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            BlueGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        }


        if (BlackGemPrefab == null)
        {
            BlackGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            BlackGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 


        
        if (GreenGemPrefab == null)
        {
            GreenGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            GreenGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 


        
        if (RedGemPrefab == null)
        {
            RedGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            RedGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 


        
        if (OrangeGemPrefab == null)
        {
            OrangeGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            OrangeGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 


        
        if (PurpleGemPrefab == null)
        {
            PurpleGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            PurpleGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 
        
    }

}
