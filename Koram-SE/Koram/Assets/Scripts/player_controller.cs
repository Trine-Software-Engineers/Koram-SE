using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_controller : MonoBehaviour
{
    public int playerDamage = 1;
    public float playerSpeed = 10; //multiplier for the height player speed- can be changed in unity scene
    public int playerJump = 20; //multiplier for the height player jumps- can be changed in unity scene
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public bool isGrounded = false; //determins if the player is able to jump
    private float moveX;
    private float joyX;
    private bool facingRight = true; //determines which way the player sprite is looking 
    private bool isAttacking = false; 

    private Animator anim; //shortens the call on the animator window
    private bool dead = false;
    public bool invincible = false;//makes player invincible for testing

    public bool shieldBlock = false;//

    protected Joystick joystick;

    void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        anim = GetComponent<Animator>();
    }

    //Every frame the player is checked for falling off the level or for having no hearts
    //Starts the die sequence if either of these perameters are met
    void FixedUpdate()
    {
        PlayerMove();
        if (gameObject.transform.position.y < -60) StartCoroutine(Die());
        if(player_hud.PlayerHealth < 1)
        {
            StartCoroutine(Die());
        }
    }

    void PlayerMove(){
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        if(dead) return;
        
        //changes player speed according to if they are walking running or crouching
        if (Input.GetButton("Walk") && !Input.GetButton("Crouch")) //Shift while moving is used for walk
        {
            playerSpeed = 2; //Player is walking       
        }
        else if (Input.GetButton("Crouch"))
        {
            playerSpeed = .2f;
        }
        else if((Input.GetButton("block")&& (!SaveManager.GetTouchScreenMode()) || (TouchShield.shieldPressed)))
        {
            playerSpeed = 0.05f;
            
        }
        else
        {
            playerSpeed = 10;  //Player is Running
        }

        //crouching function and check
        if(Input.GetButton("Crouch"))
        {
            anim.SetBool("isCrouching",true);
        }
        else
        {
            anim.SetBool("isCrouching",false);
        }

        //Blocking with sheild
        if((Input.GetButton("block")&& (!SaveManager.GetTouchScreenMode()) || (TouchShield.shieldPressed)))
        {
            anim.SetBool("isBlocking",true);
            shieldBlock = true;
        }
        else
        {
            anim.SetBool("isBlocking",false);
            shieldBlock = false;
        }

        //allows player to attack
        

        if(((Input.GetMouseButton(0) && (!SaveManager.GetTouchScreenMode()) || (TouchAttack.attackPressed)) && !isAttacking))
        {
            isAttacking = true;
            //RNG to choose attack
            int index = UnityEngine.Random.Range(1,4);
            //Debug.Log(index);

            //each of the index numbers represent an attack animation that the character can do
            if(index == 1)
            {
                anim.SetTrigger("isHitting");
                FindObjectOfType<AudioManager>().Play("sword1");
            } 
            else if(index == 2)
            {
                anim.SetTrigger("isHitting1");
                FindObjectOfType<AudioManager>().Play("sword2");
            }
            else if(index == 3)
            {
                anim.SetTrigger("isHitting2");
                FindObjectOfType<AudioManager>().Play("sword3");
            } 
            
            StartCoroutine(DoAttack());  //calls on the function for sequence of events when attack button pressed
        }  

        if(SaveManager.GetTouchScreenMode())
        {
            moveX = joystick.Horizontal;
        }
        else moveX = Input.GetAxis("Horizontal");

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
        if ((Input.GetButton("Jump") && isGrounded == true) || (TouchJump.jumpPressed && isGrounded == true)) {
            GetComponent<Rigidbody2D>().velocity = new Vector2 (gameObject.GetComponent<Rigidbody2D>().velocity.x, playerJump);
            anim.SetTrigger("isJumping"); //Playing the jump animation when player jumps
            FindObjectOfType<AudioManager>().Play("jump");
            playerSpeed = 10f;
        }

        if (GetComponent<Rigidbody2D>().velocity.y < 0)
                {
                GetComponent<Rigidbody2D>().velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }   
        else if ((GetComponent<Rigidbody2D>().velocity.y > 0) && (!Input.GetButton ("Jump")) && (!TouchJump.jumpPressed))
                {
                GetComponent<Rigidbody2D>().velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
                }
    }

    //Detects which way the sprite is currently facing and flips it if a movement is made in the opposite direction
    void FlipPlayer()
    {
        facingRight = !facingRight;
        gameObject.transform.Rotate (0f, 180, 0f);
    }  

    //collider for the end win screen when the final level is completed
    void OnTriggerEnter2D(Collider2D trig) 
    {
        //if player touches EndOfLevel, player wins
        if (trig.gameObject.name == "EndOfLevel") 
        {
            if (SceneManager.GetActiveScene().name == "Level20")
            {
                WinScreen.Final = true;
            }
            else
            {
                WinScreen.Win = true;
            }
        }
    
        //deal damage to skeleton
        EnemySkeleton skeleton = trig.GetComponent<EnemySkeleton>();
        if (skeleton != null) skeleton.TakeDamage(playerDamage);

        //deal damage to spider
        EnemySpider spider = trig.GetComponent<EnemySpider>();
        if (spider != null) spider.TakeDamage(playerDamage);
    }

    //this is for testing purposes mainly to make sure all of level is playable without worrking about being hit by enemies
    public void TakeDamage ( int damage) 
    {
        if(invincible)
        {
            Debug.Log("player is invincible for testing");
            return;
        }
        if(shieldBlock == true)
        {
            return;
        }
        player_hud.PlayerHealth -= damage;
        FindObjectOfType<AudioManager>().Play("hurt");
        if (player_hud.PlayerHealth <=0) StartCoroutine(Die());
    }

    //Sequence of timed events that happen when the player dies
    IEnumerator Die()
    {
        //play death animation
        dead = true;
        FindObjectOfType<AudioManager>().Play("Die");
        anim.SetBool("died",true);

        Audio.Stop("Level" + (SceneManager.GetActiveScene().buildIndex).ToString());
        
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Audio.Play(SceneManager.GetActiveScene().name);


        //delete current score upon death
        SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
        SaveManager.ClearCurrentScore(SceneManager.GetActiveScene().buildIndex);


        player_hud.PlayerHealth = 3;
        player_hud.TimeTaken = 0;

        // bool for death screen
        DeathScreen.GameIsDead = true;
    }

    //This IEnumerator is a sequence of events that is called upon by the player move script when the sprite is attacking
    IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(.4f); //waits 0.4 secoonds 
        isAttacking = false;
    }
}
