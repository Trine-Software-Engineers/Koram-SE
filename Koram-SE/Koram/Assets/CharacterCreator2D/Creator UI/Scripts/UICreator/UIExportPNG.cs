using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace CharacterCreator2D.UI 
{
	public class UIExportPNG : MonoBehaviour {

		[Header("UI References")]
		public Camera previewCam;
		public Animator character;
		public CharacterViewer characterViewer;
		public RawImage previewUI;
		public RectTransform previewUIContainer;
		public RectTransform previewScrollRect;
		public Dropdown baseAnimation;
		public Dropdown aimAnimation;
		public Dropdown emoteDropdown;
		public Toggle playToggle;
		public Text animationLengthUI;
		public Text frameCountUI;
		public Dropdown configPreset;
		public RuntimeDialog runtimeDialog;
		[Space]
		public InputField inputFPS;
		public InputField inputWidth;
		public InputField inputHeight;
		public Dropdown inputBGColor;
		public Dropdown inputExportMode;
		public InputField inputOffsetX;
		public InputField inputOffsetY;
		public InputField inputScale;
		public Dropdown inputScaleMode;

		[Header("Export Settings")]
		public int targetFPS = 12;
		public int width = 512;
		public int height = 512;
		public BackgroundColor bgColor = 0;
		public ExportMode exportMode = 0;
		public Vector3 charPosition = Vector3.zero;
		public Vector3 charScale = Vector3.one;
		public ScaleMode scaleMode = 0;
		public bool flipCharacter = false;
		public bool loopAimLayer = false;

		// Path and file name
		[HideInInspector]
		public string path = "";
		string fileName = "NewCharacter";

		// Export config path
		string configPath = "";
		string configExtension = ".pngconfig";

		int superSampling = 2;

		float targetLength;
		float baseLength;
		float aimLength;
		int frameCount;
		string selectedAnimation;
		string selectedAim;

		int currentPreset;
		bool isLoadingconfig;

		float PPU = 100;

		RenderTexture renderTexture;

		void OnEnable () {
			UpdateSpriteSize();
			UpdateZoomPreview(1);
			UpdateCharacter();
			SetCharFlip(flipCharacter);

			GetConfigPath();
			UpdateConfigDropdown();
			UpdateEmoteDropdown();
			GetAnimationLength();

			LayoutRebuilder.MarkLayoutForRebuild(previewScrollRect);
			LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
		}

		public void ResetCharacter () {
			Vector3 rotation = Vector3.zero;
			rotation.x = 0;
			character.transform.eulerAngles = rotation;
			character.transform.localScale = Vector3.one;
			character.transform.position = Vector3.zero;
		}

		void GetConfigPath () {
			#if UNITY_EDITOR
			configPath = Application.dataPath + "/CharacterCreator2D/Creator UI/Config/";
			#else
			configPath = Application.dataPath + "/../Config/";
			#endif
		}

		void UpdateConfigDropdown () {			
			if(!Directory.Exists(configPath))
				return;			
			DirectoryInfo dir = new DirectoryInfo(configPath);
			FileInfo[] files = dir.GetFiles("*" + configExtension);
			if(files.Length <= 0)
				return;
			configPreset.ClearOptions();
			configPreset.options.Add (new Dropdown.OptionData("Custom"));
			foreach (FileInfo f in files)
			{
				string s = f.Name.Replace(configExtension,"");
				configPreset.options.Add(new Dropdown.OptionData(s));
			}
			configPreset.RefreshShownValue();
		}

		void SetConfigToCustom () {
			if(isLoadingconfig)
				return;
			currentPreset = 0;
			configPreset.value = 0;
		}

		public void LoadConfig (int i) {
			if (i == 0)
				return;
			string configName = configPreset.options[i].text;
			LoadConfig(configName);
		}

		public void EraseConfig () {
			string configName = configPreset.options[configPreset.value].text;
			string path = configPath + configName + configExtension;
			if(File.Exists(path))
				File.Delete(path);
		}
		
		public void SaveConfig(string configName) {
			if (string.IsNullOrEmpty(configName))
				return;

			ExportConfig config = new ExportConfig();

			config.targetFPS = targetFPS;
			config.width = width;
			config.height = height;
			config.bgColor = bgColor;
			config.exportMode = exportMode;
			config.charPosition = charPosition;
			config.charScale = charScale;
			config.scaleMode = scaleMode;

			if(!Directory.Exists(configPath))
				Directory.CreateDirectory(configPath);
			string path = configPath + configName + configExtension;
			string file = JsonUtility.ToJson(config);
			File.WriteAllText(path, file);

			UpdateConfigDropdown();
			for (int i = 0; i < configPreset.options.Count; i++)
			{
				if (configPreset.options[i].text == configName) {
					currentPreset = i;
					configPreset.value = i;
					break;
				}
			}
		}

		void LoadConfig(string configName) {
			if (currentPreset == configPreset.value)
				return;

			currentPreset = configPreset.value;
			isLoadingconfig = true;

			string path = configPath + configName + configExtension;
			if(!File.Exists(path))
				return;

			ExportConfig config = JsonUtility.FromJson<ExportConfig>(File.ReadAllText(path));

			inputFPS.text = config.targetFPS.ToString();
			inputWidth.text = config.width.ToString();
			SetWidth(config.width.ToString());
			inputHeight.text = config.height.ToString();
			SetHeight(config.height.ToString());
			inputBGColor.value = (int)config.bgColor;
			inputExportMode.value = (int)config.exportMode;
			inputOffsetX.text = config.charPosition.x.ToString("0.00");
			inputOffsetY.text = config.charPosition.y.ToString("0.00");
			inputScale.text = config.charScale.x.ToString("0.00");
			inputScaleMode.value = (int)config.scaleMode;
			UpdateSpriteSize();

			isLoadingconfig = false;
		}

		public void SetWidth (string w) {
			if(string.IsNullOrEmpty(w))
				return;
			width = int.Parse(w);
			UpdateSpriteSize();
		}

		public void SetHeight (string h) {
			if(string.IsNullOrEmpty(h))
				return;
			height = int.Parse(h);
			UpdateSpriteSize();
		}

		public void SetScaleMode (int i) {
			scaleMode = (ScaleMode)i;
			UpdateCameraZoom();
		}

		void UpdateSpriteSize () {
			renderTexture = new RenderTexture(width*superSampling,height*superSampling,24,RenderTextureFormat.ARGB32);
			renderTexture.filterMode = FilterMode.Bilinear;
			previewCam.targetTexture = renderTexture;
			previewUI.texture = renderTexture;
			previewUI.SetNativeSize();
			previewUIContainer.sizeDelta = previewUI.rectTransform.sizeDelta;
			UpdateCameraZoom();
		}

		void UpdateCameraZoom () {
			if (scaleMode == ScaleMode.ActualPixelSize) previewCam.orthographicSize = height / PPU;
			else previewCam.orthographicSize = 10;
			SetConfigToCustom();
		}

		void UpdateEmoteDropdown () {
			emoteDropdown.ClearOptions();
			emoteDropdown.options.Add (new Dropdown.OptionData());
			emoteDropdown.options[0].text = "None";

			string[] s = System.Enum.GetNames(typeof(EmotionType));
			List<string> names = new List<string>(s);
			emoteDropdown.AddOptions(names);			
		}

		public void UpdateBackground (int i) {
			bgColor = (BackgroundColor)i;
			if (bgColor == BackgroundColor.Transparent)
				previewCam.backgroundColor = Color.clear;
			else if (bgColor == BackgroundColor.Black)
				previewCam.backgroundColor = Color.black;
			else if (bgColor == BackgroundColor.White)
				previewCam.backgroundColor = Color.white;
			else if (bgColor == BackgroundColor.Gray)
				previewCam.backgroundColor = Color.gray;
			else if (bgColor == BackgroundColor.Magenta)
				previewCam.backgroundColor = Color.magenta;
			else if (bgColor == BackgroundColor.Green)
				previewCam.backgroundColor = Color.green;
			SetConfigToCustom();
		}

		public void SetCharPosX (float x) {
			charPosition.x = x;
			UpdateCharacter();
		}

		public void SetCharPosY (float y) {
			charPosition.y = y;
			UpdateCharacter();
		}

		public void SetCharScale (float s) {
			charScale.x = s;
			charScale.y = s;
			UpdateCharacter();
		}

		public void SetCharFlip (bool flip) {
			flipCharacter = flip;
			Vector3 rotation = Vector3.zero;
			if (flip) rotation.y = 180;
			else rotation.y = 0;
			character.transform.eulerAngles = rotation;
		} 

		void UpdateCharacter () {
			character.transform.position = charPosition;
			character.transform.localScale = charScale;
			SetConfigToCustom();
		}

		public void SetFPS (string s) {
			targetFPS = int.Parse(s);
			CalculateFrameCount();
			SetConfigToCustom();
		}		

		public void PlayAnimation (bool play) {
			if (play) character.speed = 1;
			else character.speed = 0;
		}

		public void Emote (int i) {
			if (i-1 < 0) characterViewer.ResetEmote();
			else characterViewer.Emote((EmotionType)i-1);
		}

		public void SetAimAngle (float f) {
			character.SetFloat("Aim",f);
		}	

		public void UpdateZoomPreview (float f) {
			f /= (float)superSampling;
			previewUIContainer.localScale = new Vector3 (f,f,1);
		}

		public void SetExportMode (int i) {
			exportMode = (ExportMode)i;
		}

		public void Export () {
			if (exportMode == ExportMode.SingleImage)
				SaveAsPNG();
			else if (exportMode == ExportMode.PNGSequence)
				StartCoroutine (SaveSequence());
		}

		public void GetAnimationLength () {
			// Get target animation length in each layer
			// Base layer
			selectedAnimation = baseAnimation.options[baseAnimation.value].text;
			character.Update(0);
			AnimatorClipInfo[] info = character.GetCurrentAnimatorClipInfo(0);
			if (0 < info.Length)
			baseLength = info[0].clip.length;
			else baseLength = 0;
			//Aim layer
			selectedAim = aimAnimation.options[aimAnimation.value].text;
			character.Update(0);
			AnimatorClipInfo[] aimStateInfo = character.GetCurrentAnimatorClipInfo(1);
			if (0 < aimStateInfo.Length)
			aimLength = aimStateInfo[0].clip.length;
			else aimLength = 0;

			// Calculate the target length of the sequence
			// if aim animation is longer, and loop aim layer is turned off,
			// loop the base animation until aim animation is done
			targetLength = baseLength;
			if (targetLength <= 0 && aimLength > targetLength)
				targetLength = aimLength;
			else if (!loopAimLayer) {
				while (aimLength > targetLength)
					targetLength += baseLength;
			}

			animationLengthUI.text = targetLength.ToString();

			CalculateFrameCount();
		}

		private void CalculateFrameCount () {
			// Calculate final frame count that will be exported
			frameCount = (int)(targetLength * targetFPS);
			if (frameCount <= 0) frameCount = 1;
			frameCountUI.text = frameCount.ToString();
		}

		private void SaveAsPNG () {

			// Show save file dialog
			#if UNITY_EDITOR
			path = UnityEditor.EditorUtility.SaveFilePanel("Export Single Image", "Assets/", fileName, "png");
			#endif			

			if (string.IsNullOrEmpty(path))
				return;

			// Pause character animation
			character.speed = 0;
			playToggle.isOn = false;
			
			// Render and save to file
			Texture2D tex2D = new Texture2D (width*superSampling, height*superSampling, TextureFormat.RGBA32, false);
			RenderTexture.active = renderTexture;
			previewCam.Render();
			tex2D.ReadPixels(new Rect(0,0,width*superSampling,height*superSampling),0,0,false);
			TextureScaler.Bilinear(tex2D,width,height);
			tex2D.Apply();
			byte[] bytes = tex2D.EncodeToPNG();
			File.WriteAllBytes(path, bytes);

			RenderTexture.active = null;
			Object.Destroy(tex2D);

			// Show success dialog box
			Debug.Log ("Saved to " + path);
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.DisplayDialog("Save Successful", "Saved to " + path, "OK");
			#else
			runtimeDialog.DisplayDialog("Save Successful", "Saved to " + path);
			#endif
		}

		private IEnumerator SaveSequence () {

			// Show save dialog
			#if UNITY_EDITOR
			path = UnityEditor.EditorUtility.SaveFilePanel("Export Sequence", "Assets/", fileName, "png");
			#endif		

			if (string.IsNullOrEmpty(path))
				yield break;

			// Split path into base (without extension) and directory path to use when saving sequence
			string basePath = path.Replace(".png", "_");

			// Get animation length
			GetAnimationLength();
			float frameDelay = 1/(float)targetFPS;
			float elapsedTime = 0;

			// Pause character animation and reset aim layer to make sure muzzle flash particle is off
			playToggle.isOn = false;
			character.Play("None", 1, 0);
			yield return null;

			// Iterate and render through each frame of the sequence
			for (int i = 0; i < frameCount; i++)
			{
				yield return new WaitForSeconds(frameDelay);

				// update progress bar
				float progress = (float)i + 1 / frameCount;
				#if UNITY_EDITOR
				UnityEditor.EditorUtility.DisplayProgressBar("Export Sequence", "Exporting frame " + i.ToString() + "/" + frameCount.ToString(), progress);
				#else
				runtimeDialog.DisplayProgressBar("Export Sequence", "Exporting frame " + i.ToString() + "/" + frameCount.ToString(), progress);
				#endif

				// Set the character animation and convert animation length into normalized time				
				float baseTime = (float)i * frameDelay / baseLength;
				character.Play(selectedAnimation, 0, baseTime);
				character.speed = 0;
				// Only play aim layer once and clamp the animation at the end
				// or keep looping if loop aim layer is set to true
				if (elapsedTime <= aimLength || loopAimLayer)
				{
					float aimTIme = (float)i * frameDelay / aimLength;
					character.Play(selectedAim, 1, aimTIme);
					character.speed = 0;
				}

				// Wait til animator is updated properly
				elapsedTime += frameDelay;
				yield return null;

				// Render and save the frame
				Texture2D tex2D = new Texture2D (width*superSampling, height*superSampling, TextureFormat.RGBA32, false);
				RenderTexture.active = renderTexture;
				previewCam.Render();
				tex2D.ReadPixels(new Rect(0,0,width*superSampling,height*superSampling),0,0,false);
				TextureScaler.Bilinear(tex2D, width, height);
				tex2D.Apply();
				byte[] bytes = tex2D.EncodeToPNG();
				
				path = basePath + i.ToString("000") + ".png";
				File.WriteAllBytes(path, bytes);

				RenderTexture.active = null;
				Object.Destroy(tex2D);	
			}
			
			// Show success dialog box
			Debug.Log ("PNG sequences saved to " + path);	
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.ClearProgressBar();
			UnityEditor.EditorUtility.DisplayDialog("Save Successful", "PNG sequence saved to " + path, "OK");			
			#else
			runtimeDialog.DisplayDialog("Save Successful", "PNG sequence saved to " + path);
			#endif
				
		}

	}

	public enum ExportMode {
		SingleImage,
		PNGSequence
	}

	public enum ScaleMode {
		Automatic,
		ActualPixelSize
	}

	public enum BackgroundColor {
		Transparent,
		Black,
		White,
		Gray,
		Magenta,
		Green
	}

	public class ExportConfig {		
		public int targetFPS = 12;
		public int width = 512;
		public int height = 512;
		public BackgroundColor bgColor = 0;
		public ExportMode exportMode = 0;
		public Vector3 charPosition = Vector3.zero;
		public Vector3 charScale = Vector3.one;
		public ScaleMode scaleMode = 0;
	}

}
