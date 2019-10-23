using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace CharacterCreator2D.UI 
{
	public class InputFieldSlider : MonoBehaviour {

		public InputField inputField;
		public Slider slider;
		public string stringFormat = "0.00";
		
		public ValueChange onValueChanged;

		float value;

		bool isEditingField;
		bool isEditingSlider;
		
		void Start () {	
			if (inputField!=null) {
				inputField.text = slider.value.ToString(stringFormat);
				inputField.onValueChanged.AddListener(delegate {FieldChange();});
				inputField.onEndEdit.AddListener(delegate {EndEdit();});
			}		
			if (slider!=null) {
				value = slider.value;
				slider.onValueChanged.AddListener(delegate {SliderChange();});
			}
		}
		
		void SliderChange () {
			if (isEditingField) 
				return;
			isEditingSlider = true;
			inputField.text = slider.value.ToString(stringFormat);
			value = slider.value;
			onValueChanged.Invoke(value);
			isEditingSlider = false;
		}

		void FieldChange () {
			if (isEditingSlider)
				return;
			isEditingField = true;
			string s = inputField.text;
			float f = 0;
			if(float.TryParse(s,out f)) {
				slider.value = f;
				value = f;
				onValueChanged.Invoke(f);
			}
			isEditingField = false;
		}

		void EndEdit () {
			float f = 0;
			if(float.TryParse(inputField.text,out f)) 
				inputField.text = f.ToString(stringFormat);				
			else
				inputField.text = stringFormat;
			isEditingField = false;
		}
	}

	[System.Serializable]
	public class ValueChange : UnityEvent<float> {}
}
