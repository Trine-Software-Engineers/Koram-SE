using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlocker : MonoBehaviour
{

    public Button[] buttons;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        for(int x = 0; x < buttons.Length; x++)
        {
            if (SaveManager.GetLevelsCompleted() + 1 > x)
            {
                buttons[x].interactable = true;
            }
            else buttons[x].interactable = false;

            
        }
    }

    
}
