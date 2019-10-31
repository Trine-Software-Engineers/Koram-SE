using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    //Movement
    public int PlayerSpeed = 10;
    public int PlayerJump = 10;
    public bool isGrounded = false;
    private float MoveX;
    private bool FacingRight = true;
    
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
        if (Input.GetButtonDown("Jump") && isGrounded == true) GetComponent<Rigidbody2D>().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.x, PlayerJump);
    }

    void FlipPlayer()
    {
        FacingRight = !FacingRight;
        gameObject.transform.Rotate (0f, 180, 0f);
    }

    void OnCollisionEnter2D(Collision2D collision){
        GameObject myEnemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyFollower enemyScript = myEnemy.GetComponent<enemyFollower>();
        if (collision.collider.tag == "Enemy"){
            player_hud.PlayerHealth -= enemyScript.damage; 
            Debug.Log(player_hud.PlayerHealth);
        }
    }

    public void TakeDamage ( int damage) {
        player_hud.PlayerHealth -= damage;
        if (player_hud.PlayerHealth <=0){
            Die();
        }
    }
  
    void Die(){
        Destroy(gameObject);
    }

}
