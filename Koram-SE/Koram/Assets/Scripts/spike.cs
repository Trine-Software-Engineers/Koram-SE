using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike : MonoBehaviour
{
  public static int spikeDamage = 1;

void OnTriggerEnter2D(Collider2D trig) 
{
    player_controller player = trig.GetComponent<player_controller>();
    if (player != null) player.TakeDamage(spikeDamage); 
    Debug.Log("spike");
}
}
