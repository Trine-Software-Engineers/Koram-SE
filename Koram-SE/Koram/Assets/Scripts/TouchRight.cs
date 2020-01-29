using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public static bool rightPressed;

 
public void OnPointerDown(PointerEventData eventData){
    if(this.gameObject.name == ("RightButton")) rightPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    if(this.gameObject.name == ("RightButton")) rightPressed = false;
}


}


