using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchShield : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public static bool shieldPressed;

 
public void OnPointerDown(PointerEventData eventData){
    if(this.gameObject.name == ("ShieldButton")) shieldPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    if(this.gameObject.name == ("ShieldButton")) shieldPressed = false;
}


}


