using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
 
public static bool backwardpressed;


 
public void OnPointerDown(PointerEventData eventData){
    Debug.Log(this.gameObject.name + " Was Clicked.");
    if(this.gameObject.name == ("BackButton")) backwardpressed = true;
}
 
public void OnPointerUp(PointerEventData eventData){
    if(this.gameObject.name == ("BackButton")) backwardpressed = false;
}



void Update()
{
    if(backwardpressed) Debug.Log("moving backwards");

}


}


