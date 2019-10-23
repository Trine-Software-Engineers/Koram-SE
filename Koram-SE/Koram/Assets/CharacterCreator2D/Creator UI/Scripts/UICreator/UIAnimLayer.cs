using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterCreator2D.UI
{
	public class UIAnimLayer : MonoBehaviour {

		public GameObject[] toggleObjects;
		public bool defaultActive;

		bool toggle; 
		
		void Start () {
			toggle = !defaultActive;
			Toggle();
		}
		
		public void Toggle () {
			foreach (GameObject go in toggleObjects)
			{
				go.SetActive(!toggle);
			}
			toggle = !toggle;
		}

	}
}
