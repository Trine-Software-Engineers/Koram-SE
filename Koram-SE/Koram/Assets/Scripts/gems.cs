using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gems : MonoBehaviour
{
    public GameObject pickupEffect;
    private GameObject gem;
    bool pick = false;
//This creates an event that happens when an object tagged player collides with the gems
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")&& pick == false)
        {
            pick = true;
            StartCoroutine(Pickup(other));
        }
    }

    IEnumerator Pickup(Collider2D player)
    {
        //Spawn extra gem effect
        var hearteffect = Instantiate(pickupEffect, transform.position, transform.rotation);

        //apply extra gem
        FindObjectOfType<AudioManager>().Play("gem");
        WinScreen.score += 200;
        

        yield return new WaitForSeconds(1f);
        //remove powerup sprite
        Destroy(gameObject);
        Destroy(hearteffect);
        pick = false;
    }
}