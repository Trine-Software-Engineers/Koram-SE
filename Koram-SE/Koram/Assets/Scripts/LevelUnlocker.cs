using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelUnlocker : MonoBehaviour
{

    public Button[] buttons;

    public GameObject[] levelScore;
    public GameObject totalScore;

    // Start is called before the first frame update
    void Awake()
    {
        buttons = new Button[21];
        for(int x = 1; x < 21; x++)
        {
            Button _button = GameObject.Find("Level" + x).GetComponent<Button>();
            buttons[x] = _button;
        }


        levelScore = new GameObject[21];
        for(int x = 1; x < 21; x++)
        {
            levelScore[x] = GameObject.Find("Level" + x + "Score");
        }

        totalScore = GameObject.Find("TotalScore");



        for(int x = 1; x < 21; x++)
        {
            levelScore[x].GetComponent<TextMeshProUGUI>().text = ""; 
        }
        
        totalScore.GetComponent<TextMeshProUGUI>().text = "";
    }


    // Update is called once per frame
    void Update()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        for(int x = 1; x < 21; x++)
        {
            if (SaveManager.GetLevelsCompleted() + 1 >= x)
            {
                buttons[x].interactable = true;
            }
            else buttons[x].interactable = false;

            if(SaveManager.GetBestScore(x) > 0) levelScore[x].GetComponent<TextMeshProUGUI>().text = (SaveManager.GetBestScore(x)).ToString();
            else levelScore[x].GetComponent<TextMeshProUGUI>().text = "";

           
        }

    


        totalScore.GetComponent<TextMeshProUGUI>().text = (SaveManager.GetFinalOverallScore()).ToString();
    }

    
}
