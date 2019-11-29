using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //These functions see if the the sprite is in contact with an object tagged ground
    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Ground" || collision.collider.tag == "Enemy")
            {
            Player.GetComponent<player_controller>().isGrounded = true;
            }
        }
    private void OnCollisionExit2D(Collision2D collision) 
    {
        if (collision.collider.tag == "Ground" || collision.collider.tag == "Enemy")
        {
            Player.GetComponent<player_controller>().isGrounded = false;
        }

    }
}