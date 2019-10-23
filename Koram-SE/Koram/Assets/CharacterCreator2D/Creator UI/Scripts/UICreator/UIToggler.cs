using UnityEngine;
using UnityEngine.UI;

namespace CharacterCreator2D.UI
{
	public class UIToggler : MonoBehaviour {

		public bool active = true;
		public Image icon;
		public Sprite expandedIcon;
		public Sprite collapsedIcon;
		public GameObject[] objects;

		void Start () {
			active = !active;
			Toggle();
		}

		public void Toggle () {
			active = !active;
			foreach (GameObject go in objects) go.SetActive(active);
			if (icon == null) return; 
			if (active) icon.sprite = expandedIcon;
			else icon.sprite = collapsedIcon;
		}
	}
}