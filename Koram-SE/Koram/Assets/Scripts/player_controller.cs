using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_controller : MonoBehaviour
{
    public int playerDamage = 1;
    public float playerSpeed = 10; //multiplier for the height player speed- can be changed in unity scene
    public int playerJump = 20; //multiplier for the height player jumps- can be changed in unity scene
    public bool isGrounded = false; //determins if the player is able to jump
    private float moveX;
    private bool facingRight = true; //determines which way the player sprite is looking 
    private bool isAttacking = false; 

    private Animator anim; //shortens the call on the animator window
    
    private bool dead = false;
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
        if (gameObject.transform.position.y < 0) Die();
        if(player_hud.PlayerHealth < 1)
        {
            StartCoroutine(Die());
        }
    }

    void PlayerMove(){
        if(dead) return;
        
        if (Input.GetButton("Walk") && !Input.GetButton("Crouch")) //Shift while moving is used for walk
        {
            playerSpeed = 2; //Player is walking       
        }
        else if (Input.GetButton("Crouch"))
        {
            playerSpeed = 0.1f;
        }
        else
        {
            playerSpeed = 10;  //Player is Running
        }

        if(Input.GetButton("Crouch"))
        {
            anim.SetBool("isCrouching",true);
        }
        else
        {
            anim.SetBool("isCrouching",false);
        }

        if(Input.GetButton("Fire1") && !isAttacking)
        {
            isAttacking = true;
            //RNG to choose attack
            int index = UnityEngine.Random.Range(1,7);
            //Debug.Log(index);

            //each of the index numbers represent an attack animation that the character can do
            if(index == 1)
            {
                anim.SetTrigger("isHitting");
            } 
            else if(index == 2)
            {
                anim.SetTrigger("isHitting1");
            }
            else if(index == 3)
            {
                anim.SetTrigger("isHitting2");
            } 
            else if(index == 4)
            {
                anim.SetTrigger("isHitting3");
            } 
            else if(index == 5)
            {
                anim.SetTrigger("isHitting4");
            } 
            else if (index == 6)
            {
                anim.SetTrigger("isHitting5");
            }  
            StartCoroutine(DoAttack());  //calls on the function for sequence of events when attack button pressed
        }  

        moveX = Input.GetAxis("Horizontal");
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (moveX * playerSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
       

        if (moveX != 0.0f && Input.GetButton("Walk")) //if shift is down and sprite is moving it is walking
        {
            anim.SetBool("isRunning",false);
            anim.SetBool("isWalking",true);
        }
        else if (moveX != 0.0f ) //default sprint for this game 
        {   
            anim.SetBool("isRunning",true);
        }
        else //idle state
        {
            anim.SetBool("isRunning",false);   
            anim.SetBool("isWalking",false);
        }

        //Flip Sprite
        if (moveX < 0.0f && facingRight == true) FlipPlayer();
        else if (moveX > 0.0f && facingRight == false) FlipPlayer();

        //Jumping
        if (Input.GetButton("Jump") && isGrounded == true){
            GetComponent<Rigidbody2D>().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.x, playerJump);
            anim.SetTrigger("isJumping"); //Playing the jump animation when player jumps
            FindObjectOfType<AudioManager>().Play("jump");
        }
        
    }
    //Detects which way the sprite is currently facing and flips it if a movement is made in the opposite direction
    void FlipPlayer()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate (0f, 180, 0f);
    }  
    void OnTriggerEnter2D(Collider2D trig) 
    {
        //if player touches EndOfLevel, player wins
        if (trig.gameObject.name == "EndOfLevel") WinScreen.Win = true;
    
        //deal damage to skeleton
        EnemySkeleton skeleton = trig.GetComponent<EnemySkeleton>();
        if (skeleton != null) skeleton.TakeDamage(playerDamage);

        //deal damage to spider
        EnemySpider spider = trig.GetComponent<EnemySpider>();
        if (spider != null) spider.TakeDamage(playerDamage);
    }

    public void TakeDamage ( int damage) 
    {
        if(invincible)
        {
            Debug.Log("player is invincible for testing");
            return;
        }

        player_hud.PlayerHealth -= damage;
        if (player_hud.PlayerHealth <=0) StartCoroutine(Die());
    }

    IEnumerator Die(){
        //play death animation
        dead = true;
        anim.SetBool("died",true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        SceneManager.LoadScene("Main");   
    }

    //This IEnumerator is a sequence of events that is called upon by the player move script when the sprite is attacking
    IEnumerator DoAttack()
        {
            yield return new WaitForSeconds(.4f); //waits 0.4 secoonds 
            isAttacking = false;
        }
}
