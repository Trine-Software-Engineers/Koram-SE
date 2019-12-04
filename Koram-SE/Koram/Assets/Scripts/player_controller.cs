using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_controller : MonoBehaviour
{
    //Movement
    public float PlayerSpeed = 10; //multiplier for the height player speed- can be changed in unity scene
    public int PlayerJump = 10; //multiplier for the height player jumps- can be changed in unity scene
    public bool isGrounded = false; //determins if the player is able to jump
    private float MoveX;
    private bool FacingRight = true; //determines which way the player sprite is looking 
    private Animator anim; //shortens the call on the animator window
    
    public bool invincible = false;//makes player invincible for testing

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        if (Input.GetButton("Walk")) //Shift while moving is used for walk
        {
            PlayerSpeed = 2; //Player is walking        
        }
        else
        {
            PlayerSpeed = 10;  //Player is Running
        }


        MoveX = Input.GetAxis("Horizontal");
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (MoveX * PlayerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
       

        if (MoveX != 0.0f && Input.GetButton("Walk")) //if shift is down and sprite is moving it is walking
        {
            anim.SetBool("isRunning",false);
            anim.SetBool("isWalking",true);
        }
        else if (MoveX != 0.0f ) //default sprint for this game 
        {   
            anim.SetBool("isRunning",true);
        }
        else //idle state
        {
            anim.SetBool("isRunning",false);   
            anim.SetBool("isWalking",false);
        }

        //Flip Sprite
        if (MoveX < 0.0f && FacingRight == true) FlipPlayer();
        else if (MoveX > 0.0f && FacingRight == false) FlipPlayer();

        //Jumping
        if (Input.GetButton("Jump") && isGrounded == true){
            GetComponent<Rigidbody2D>().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.x, PlayerJump);
            anim.SetTrigger("isJumping"); //Playing the jump animation when player jumps
        }
        
    }
    //Detects which way the sprite is currently facing and flips it if a movement is made in the opposite direction
    void FlipPlayer()
    {
        FacingRight = !FacingRight;
        gameObject.transform.Rotate (0f, 180, 0f);
    }
    //function to detect when the character comes in contact with object tagged enemy    
    void OnCollisionEnter2D(Collision2D collision){
        GameObject myEnemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyFollower script = gameObject.GetComponent<enemyFollower>();
        if (script != null)
        {
            enemyFollower enemyScript = myEnemy.GetComponent<enemyFollower>();

            if (collision.collider.tag == "Enemy")
            {
                player_hud.PlayerHealth -= enemyScript.damage; 
                Debug.Log(player_hud.PlayerHealth);
            }
        }         
    }
    
    //if player touches EndOfLevel, player wins
    void OnTriggerEnter2D(Collider2D trig) 
    {
        if (trig.gameObject.name == "EndOfLevel") 
        {
            WinScreen.Win = true;
        }
    }

    public void TakeDamage ( int damage) {
        if(invincible){
            Debug.Log("player is invincible for testing");
            return;
        }

        player_hud.PlayerHealth -= damage;
        if (player_hud.PlayerHealth <=0) Die();
    }

    void Die(){
        //play death animation
        dead = true;
        anim.SetBool("died",true);

        Audio.Stop(SceneManager.GetActiveScene().name);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        SceneManager.LoadScene("Main");  

        player_hud.PlayerHealth = 3;
        player_hud.MaxHealth = 3; 
        player_hud.TimeTaken = 0;
    }
}
