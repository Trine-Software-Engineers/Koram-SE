using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_enemy : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 1;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D (Collider2D hitInfo) {
        player_controller player = hitInfo.GetComponent<player_controller>();
        if (player != null){
            player.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
