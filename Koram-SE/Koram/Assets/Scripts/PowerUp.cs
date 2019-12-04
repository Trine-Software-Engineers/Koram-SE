using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;
    private GameObject Heart;
    bool picked = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && picked ==false)
        {
            picked = true;
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D player)
    {
        //Spawn extra heart effect
        var hearteffect = Instantiate(pickupEffect, transform.position, transform.rotation);

        //apply extra hearts
        FindObjectOfType<AudioManager>().Play("oneup");
        player_hud.MaxHealth += 1;
        player_hud.PlayerHealth += 1;

        yield return new WaitForSeconds(1f);
        //remove powerup sprite
        Destroy(gameObject);
        Destroy(hearteffect);
        picked = false;
    }
}