using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public static bool menuPressed;

 
public void OnPointerDown(PointerEventData eventData){
    if(this.gameObject.name == ("MenuButton")) menuPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    if(this.gameObject.name == ("MenuButton")) menuPressed = false;
}


}


