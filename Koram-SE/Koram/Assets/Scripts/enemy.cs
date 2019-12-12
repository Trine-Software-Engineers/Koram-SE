using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public float speed;
    public int damage;  
    public bool movingRight = true;
    public Transform groundDetection;
    public int health;

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
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 1f);
        if(groundInfo.collider == false) {
            if(movingRight == true) {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            } else {
                transform.eulerAngles = new Vector3(0,0,0);
                movingRight = true;
            }
        }
    }


}
