﻿using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public LevelFader transition;
    public void Select (string levelName)
    {
        transition.FadeTo (levelName);
    }
}
