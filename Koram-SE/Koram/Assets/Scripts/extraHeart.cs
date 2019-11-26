using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class extraHeart : MonoBehaviour
{
    public GameObject pickupEffect;
    private GameObject heart;
    public float duration = 2f;
    void OnTriggerEnter2D (Collider2D other) {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D Player)
    {
        //spawn effect
        var hearteffect = Instantiate(pickupEffect, transform.position, transform.rotation);

        //Apply to player
        player_hud.MaxHealth += 1;
        player_hud.PlayerHealth += 1;
        

        //Remove powerup
        yield return new WaitForSeconds (.8f);
        Destroy(hearteffect);
        Destroy(gameObject);
    }

    
}
