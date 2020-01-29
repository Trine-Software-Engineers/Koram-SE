using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelFader : MonoBehaviour {

	public Image img;
	public AnimationCurve curve;

	void Start ()
	{
		StartCoroutine(FadeIn());
	}

	public void FadeTo (string scene)
	{
		Debug.Log("started");
		
		StartCoroutine(FadeOut(scene));
		
		Debug.Log("done");
	}

	//This is used for a screen transition by changing the alpha values(opacity) on an image that is specified in the inspector
	//It is a transition from a scene to the image before transitioning to the next scene
	IEnumerator FadeIn ()
	{
		float t = 1f;
		while (t > 0f)
		{
			t -= Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color (0f, 0f, 0f, a);
			yield return 0;
		}
	}

	//This is the same as the functiuon above only it is going from the image to the scene that is loaded
	IEnumerator FadeOut(string scene)
	{
		float t = 0f;
		while (t < 1f)
		{
			t += Time.deltaTime;
			float a = curve.Evaluate(t);
			img.color = new Color(0f, 0f, 0f, a);
			yield return 0;
		}
		SceneManager.LoadScene(scene);
	}
}