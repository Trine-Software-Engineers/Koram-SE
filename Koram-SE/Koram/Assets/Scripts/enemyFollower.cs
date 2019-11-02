using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyFollower : MonoBehaviour
{
     public float speed;

    

    public int damage;
    public int health;

    public GameObject player;

    public void TakeDamage ( int damage) {
        health -= damage;
        if (health <=0){
            Die();
        }
    }
  
    void Die(){
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if(player.transform.position.x > gameObject.transform.position.x){
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        } else {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        
    }

}
