using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*for (int i = 0; i<1000; i++;) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.y - 5, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            bulletPosition--;
        } else if (bulletPosition < -1000) {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.y + 5, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            bulletPosition++;
        }*/
    }

    void OnCollisionEnter(Collision collisionInfo){
        if (collisionInfo.collider.tag == "Player"){
            player_hud.PlayerHealth--;
        }
    }
}
