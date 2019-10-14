using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public float speed = 1;
    Rigidbody2D body_enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        body_enemy = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       //Always move forward
        Vector2 myVel = body_enemy.velocity;
        myVel.x = speed;
        body_enemy.velocity = myVel;

    }
}
