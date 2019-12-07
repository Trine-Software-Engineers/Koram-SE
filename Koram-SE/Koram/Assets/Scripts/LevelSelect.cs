using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public LevelFader transition;

    //The music for a level is selected and the menu theme is stopped
    //This is mainly used when using the level selector
    public void Select (string levelName)
    {
        transition.FadeTo (levelName);
        Audio.Stop("MenuTheme");
        Audio.Play(levelName.ToString());
    }
}
