using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    public static int PlayerNum = 0;
    // Update is called once per frame
    public void CharacterSelectFunction(int selectedNum)
    {
        PlayerNum = selectedNum;
    }
}
