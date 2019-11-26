using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_controller : MonoBehaviour
{
    //Movement
    public AudioClip JumpClip;
    public AudioSource JumpSource; 
    public float PlayerSpeed = 10; //multiplier for the height player speed- can be changed in unity scene
    public int PlayerJump = 10; //multiplier for the height player jumps- can be changed in unity scene
    public bool isGrounded = false; //determins if the player is able to jump
    private float MoveX;
    private bool FacingRight = true, isAttacking = false; //determines which way the player sprite is looking 
    private Animator anim; //shortens the call on the animator window

    [SerializeField] //show field in unity inspector
    GameObject attackHitBox; //used to enable and disable the hitbox collider when attacking
    
    private bool dead = false;
    public bool invincible = false;//makes player invincible for testing

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attackHitBox.SetActive(false);
        JumpSource.clip = JumpClip;
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

        if (Input.GetButton("Walk") && !Input.GetButton("Crouch")) //Shift while moving is used for walk
        {
            PlayerSpeed = 2; //Player is walking        
        }
        else if (Input.GetButton("Crouch"))
        {
            PlayerSpeed = 0.1f;
        }
        else
        {
            PlayerSpeed = 10;  //Player is Running
        }

        if(Input.GetButton("Crouch"))
        {
            anim.SetBool("isCrouching",true);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            anim.SetBool("isCrouching",false);
              gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        if(Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            //RNG to choose attack
            int index = UnityEngine.Random.Range(1,7);
            Debug.Log(index);
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
            JumpSource.Play();
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
        if (player_hud.PlayerHealth <=0) {
            StartCoroutine(Die());
            
        }
    }

    IEnumerator Die(){
        //play death animation
        anim.SetBool("died",true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        SceneManager.LoadScene("Main");   
    }

    //This IEnumerator is a sequence of events that is called upon by the player move script when the sprite is attacking
    IEnumerator DoAttack()
        {
            attackHitBox.SetActive(true); //enables collider for damage
            yield return new WaitForSeconds(.4f); //waits 0.4 secoonds 
            attackHitBox.SetActive(false); //disables collider for damage
            isAttacking = false;
        }
}
