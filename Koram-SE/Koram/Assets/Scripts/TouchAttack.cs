using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchAttack : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public static bool attackPressed;

 
public void OnPointerDown(PointerEventData eventData){
    if(this.gameObject.name == ("AttackButton")) attackPressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    if(this.gameObject.name == ("AttackButton")) attackPressed = false;
}



void Update()
{

}


}


