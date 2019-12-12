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

    //These functions see if the the main character or player is in contact with an object tagged ground or enemy
    //This is used so that there is not infinate jump and the player can only jump when in contact with the enemy or ground
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