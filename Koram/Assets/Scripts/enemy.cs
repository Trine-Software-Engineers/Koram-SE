using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{

    public float speed = 2;
    Rigidbody2D body_enemy;
    Transform transform_enemy;

    public int damage = 5;//change to non static
    
    // Start is called before the first frame update
    void Start()
    {
        body_enemy = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameObject.transform.position.x == 3) speed *= -1;//attempting to reverse direction
        
        //Always move forward
        Vector2 myVel = body_enemy.velocity;
        myVel.x = speed;
        body_enemy.velocity = myVel;
    }
}
