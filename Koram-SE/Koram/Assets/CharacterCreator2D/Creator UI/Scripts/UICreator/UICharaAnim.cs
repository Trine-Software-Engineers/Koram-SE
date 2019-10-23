using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterCreator2D.UI
{
    public class UICharaAnim : MonoBehaviour
    {
        public Dropdown dropDown;
        public AnimationList animationList;

        private int _sanim;
        private Animator animator;

        void Start()
        {            
            dropDown.options.Clear();
            for (int i = 0; i < animationList.stateName.Count; i++)
            {
                dropDown.options.Add(new Dropdown.OptionData(animationList.stateName[i]));
            }
            dropDown.RefreshShownValue();
            animator = GameObject.FindObjectOfType<CharacterViewer>().GetComponent<Animator>();
            _sanim = 0;
            SelectAnimation(_sanim);
        }

        private void OnEnable() 
        {
            SelectAnimation(_sanim);
        }

        public void SelectAnimation(int index)
        {
            if (index < 0 || index >= animationList.stateName.Count || animator == null)
                return;
            _sanim = index;            
            animator.Play(animationList.stateName[_sanim], animationList.animatorLayer);
        }
    }
}