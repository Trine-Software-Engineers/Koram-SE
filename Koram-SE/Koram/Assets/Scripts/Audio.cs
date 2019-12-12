using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this script sets up the play, stop and volume controls for the music and sound effects
public class Audio : MonoBehaviour
{
    public static void Play(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

    public static void Stop(string name)
    {
        FindObjectOfType<AudioManager>().Stop(name);
    }

    public static void Volume(string name, float volume)
    {
        FindObjectOfType<AudioManager>().Volume(name, volume);
    }
}
