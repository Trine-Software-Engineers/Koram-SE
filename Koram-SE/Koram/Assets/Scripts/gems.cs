using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gems : MonoBehaviour
{
    public GameObject pickupEffect;
    private GameObject gem;
    bool pick = false;

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
        

        yield return new WaitForSeconds(1f);
        //remove powerup sprite
        Destroy(gameObject);
        Destroy(hearteffect);
        pick = false;
    }
}