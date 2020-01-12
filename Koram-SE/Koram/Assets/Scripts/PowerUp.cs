using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject pickupEffect;
    private GameObject Heart;
    bool picked = false;

    //This is a trigger or collider that is set off by becoming in contact with an object tagged player
    //Can not be set off by an enemy and will remain until player touches it
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
        player_hud.PlayerHealth += 1;

        yield return new WaitForSeconds(1f);
        //remove powerup sprite
        Destroy(gameObject);
        Destroy(hearteffect);
        picked = false;
    }
}