using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player_controller : MonoBehaviour
{
    //Movement
    public int PlayerSpeed = 10;
    public int PlayerJump = 10;
    public bool isGrounded = false;
    private float MoveX;
    private bool FacingRight = true;

    public player_hud PlayerHud;
    
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

        if (MoveX != 0.0f)
        {
            gameObject.GetComponent<Animator>().SetBool("isRunning",true);
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("isRunning",false);   
        }

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
            PlayerHud.PlayerHealth -= enemyScript.damage; 
            Debug.Log(PlayerHud.PlayerHealth);
        }
    }

    public void TakeDamage ( int damage) {
        PlayerHud.PlayerHealth -= damage;
        if (PlayerHud.PlayerHealth <=0){
            Die();
        }
    }
  
    void Die(){
        Destroy(gameObject);
        SceneManager.LoadScene("level1");
    }

}
