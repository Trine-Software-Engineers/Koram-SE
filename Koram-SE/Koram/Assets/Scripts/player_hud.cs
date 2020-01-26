using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player_hud : MonoBehaviour
{
    public int CurrentHealth = 3; // only used in inspector DO NOT USE IN CODE
    //public static int MaxHealth = 3;  // supports up to 10 hearts
    public static float TimeTaken = 0;

    public static int PlayerHealth = 3; // actual player health
    public Image[] hearts;
    public Sprite HeartFull;
    public Sprite HeartEmpty;
    public GameObject TimeTakenUI;
    public GameObject currentScoreUI;
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
        hearts = new Image[10];

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

        hearts = new Image[10];

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
        currentScoreUI = GameObject.Find("CurrentScore");
    }

    //Checks players health every frame
    void Update()
    {


        //update damage taken
        if(PlayerHealth != CurrentHealth) CurrentHealth = PlayerHealth;

        //update inspector health change
        if(PlayerHealth != CurrentHealth) PlayerHealth = CurrentHealth;


        for (int i = 0; i < hearts.Length; i++)
        {
            if(i < PlayerHealth)
            {
                hearts[i].sprite = HeartFull;
            } 


            if(i < PlayerHealth)
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

            SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
            if(currentScoreUI != null) currentScoreUI.gameObject.GetComponent<Text>().text = ("Score: " + (SaveManager.GetCurrentScore(SceneManager.GetActiveScene().buildIndex).ToString()));
        }

        GemCheck();
        
    }

    //This checks if the gem is still in the level or if it is collected by the player yet
    //The greyed out version in the HUD means that the gem is not collected
    void GemCheck()
    {
        if (BlueGemPrefab == null)//blue gem check
        {
            BlueGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            BlueGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        }

        if (BlackGemPrefab == null)//black gem check
        {
            BlackGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            BlackGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 
        
        if (GreenGemPrefab == null)//green gem check
        {
            GreenGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            GreenGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 
        
        if (RedGemPrefab == null)//red gem check
        {
            RedGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            RedGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 
        
        if (OrangeGemPrefab == null)//orange gem check
        {
            OrangeGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            OrangeGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        } 
        
        if (PurpleGemPrefab == null)//purple gem check
        {
            PurpleGem.GetComponent<Image>().color = new Color32(255,255,225,255);
        }
        else 
        {
            PurpleGem.GetComponent<Image>().color = new Color32(255,255,225,50);
        }        
    }
}
