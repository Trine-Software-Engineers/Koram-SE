using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D collider){
        player_controller player = collider.GetComponent<player_controller>();
        if (player != null){
            player.TakeDamage(damage);
            Debug.Log("Hit");
        }
        Destroy(gameObject);
    }
}
