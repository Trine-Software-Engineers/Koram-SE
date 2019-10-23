using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterCreator2D.UI
{
	public class RuntimeDialog : MonoBehaviour 
	{
        [HideInInspector] public bool isActive = false;
        [HideInInspector] public bool isConfirmed = false;
        
        /// <summary>
        /// UI Text displaying the title.
        /// </summary>
        [Tooltip("UI Text displaying the title")]
		public Text title;
        /// <summary>
        /// UI Text displaying the message.
        /// </summary>
        [Tooltip("UI Text displaying the message")]
		public Text message;
        /// <summary>
        /// Parent object of the buttons.
        /// </summary>
        [Tooltip("Parent object of the buttons")]
        public GameObject buttons;
        /// <summary>
        /// Parent object of the progress bar.
        /// </summary>
        [Tooltip("Parent object of the progress bar")]
        public GameObject progressBar;
        /// <summary>
        /// UI Image displaying the progress.
        /// </summary>
        [Tooltip("UI Image displaying the progress")]
        public RectTransform progressBarFill;
        
        public GameObject okButton;
        public GameObject yesButton;
        public GameObject noButton;
                
        /// <summary>
        /// Display message.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Message to display</param>
		public void DisplayDialog(string title, string message)
		{
            DisplayDialog(title, message, false);
		}

        /// <summary>
        /// Display message.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Message to display</param>
        /// <param name="choice">Is it a yes or no dialog?</param>
        public void DisplayDialog(string title, string message, bool choice)
        {
            isActive = true;
            isConfirmed = false;
            progressBar.SetActive(false);
            buttons.SetActive(true);
			this.title.text = title;
			this.message.text = message;
            setButtonChoice(choice);
			this.gameObject.SetActive (true);            
        }

        /// <summary>
        /// Set button choice for yes or no dialog.
        /// </summary>
        void setButtonChoice (bool choice)
        {
            okButton.SetActive(!choice);
            yesButton.SetActive(choice);
            noButton.SetActive(choice);
        }
        
        /// <summary>
        /// Display progressbar.
        /// </summary>
        /// <param name="title">Title to display.</param>
        /// <param name="message">Message to display</param>
        /// <param name="progress">current progress from 0 to 1</param>
        public void DisplayProgressBar(string title, string message, float progress)
		{
            isActive = true;
            isConfirmed = false;
            progressBar.SetActive(true);
            buttons.SetActive(false);
            progressBarFill.localScale = new Vector3 (progress, 1, 1);
			this.title.text = title;
			this.message.text = message;
            setButtonChoice(false);
			this.gameObject.SetActive (true);
		}

        /// <summary>
        /// Close RuntimeDialog.
        /// </summary>
		public void Close()
		{
			this.gameObject.SetActive (false);
            isActive = false;
		}

        /// <summary>
        /// Close RuntimeDialog and set as confirmed.
        /// </summary>
        public void Confirm () 
        {
            isConfirmed = true;
			this.gameObject.SetActive (false);
            isActive = false;
        }
	}
}