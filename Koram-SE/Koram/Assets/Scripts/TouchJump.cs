using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchJump : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public static bool jumpPressed;

 
public void OnPointerDown(PointerEventData eventData){
    if(this.gameObject.name == ("JumpButton")) jumpPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    if(this.gameObject.name == ("JumpButton")) jumpPressed = false;
}


}


