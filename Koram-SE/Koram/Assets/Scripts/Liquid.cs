using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    
    public int LiquidDamage;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D trig) 
    {
        player_controller player = trig.collider.GetComponent<player_controller>();
        if (player != null) player.TakeDamage(LiquidDamage);   
        if (trig.collider.tag == "Player") Debug.Log(trig.collider.name);
    }



}
