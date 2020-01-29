using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchLeft : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public static bool leftPressed;

 
public void OnPointerDown(PointerEventData eventData){
    if(this.gameObject.name == ("LeftButton")) leftPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    if(this.gameObject.name == ("LeftButton")) leftPressed = false;
}


}


