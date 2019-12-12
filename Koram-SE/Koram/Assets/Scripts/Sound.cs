using UnityEngine.Audio;
using UnityEngine;

//This makes all of these show in inspector
[System.Serializable]

//This function makes the sliders and customizations 
public class Sound {
	public string name;
	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f; //volume of sound/music
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f; //pitch of sound/music
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false; //if track loops or plays once

	public AudioMixerGroup mixerGroup;

	[HideInInspector]
	public AudioSource source;
}
