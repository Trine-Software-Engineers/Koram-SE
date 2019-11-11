using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;
    public int multiplier = 1;

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            Pickup(other);
        }
    }

    void Pickup(Collider2D player){
        //Instantiate(pickupEffect, transform.position, transform.rotation);

        player_hud hud = player.GetComponent<player_hud>();
        hud.CurrentHealth += multiplier;

        Destroy(gameObject);

    }
}


/*
==========If it's a temporary effect==========

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;
    public int multiplier = 1;
    public float duration = 4f;

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            StartCoroutine Pickup(other);
        }
    }

    IEnumerator Pickup(Collider2D player){
        Instantiate(pickupEffect, transform.position, transform.rotation);

        player_hud hud = player.GetComponent<player_hud>();
        hud.CurrentHealth += multiplier;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collder2D>().enabled = false;
        
        yield return new WaitForSeconds(duration);
        hud.CurrentHealth -= multiplier;

        Destroy(gameObject);

    }
}
*/