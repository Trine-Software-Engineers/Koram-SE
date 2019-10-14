using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    //Movement
    public int PlayerSpeed = 10;
    public int PlayerJump = 10;
    private float MoveX;
    private bool FacingRight = true;
    
    //Combat
    public int playerHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove(){
        MoveX = Input.GetAxis("Horizontal");
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (MoveX * PlayerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);

        //Flip Sprite
        if (MoveX < 0.0f && FacingRight == true) FlipPlayer();
        else if (MoveX > 0.0f && FacingRight == false) FlipPlayer();

        //Jumping
        if (Input.GetButtonDown("Jump")) GetComponent<Rigidbody2D>().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.x, PlayerJump);
    }

    void FlipPlayer()
    {
        Vector2 theScale = gameObject.transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        FacingRight = !FacingRight;
    }

    void OnCollisionEnter2D(Collision2D collision){
        
        if (collision.collider.tag == "Enemy"){
            player_hud.PlayerHealth -= 5;
            Debug.Log(player_hud.PlayerHealth);
        }
    }

}
