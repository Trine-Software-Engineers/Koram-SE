using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpider : MonoBehaviour
{
    [Header("Spider Settings:")]
    [Tooltip("1 = normal, 2 = hard, 3 = insane")]
    public int difficulty = 1;
    [Tooltip("Time in seconds for spider to pace.")]
    public int pacingTime = 4;
    [Tooltip("Spider movement speed")]
    public float spiderSpeed = 1;
    [Header("Spider Debug:")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    [Tooltip("Assigns Automatically")]
    public GameObject targetObject;
    public bool spiderDead = false;
    [SerializeField]
    private int spiderHealth = 1;
    //movement
    private bool facingRight = false;
    private float spiderPace;
    private float spiderSpeedMultiplier = 1;
    //target detection
    private bool targetInSights = false;
    private bool aimForHead = false;
    private float spiderSightDistance = 12f;
    int layerMask = ~(1 << 8); //raycast ignores all but player layer
    int groundLayerMask = (1 << 11); //raycast ignores all but ground layer
    //animations
    private bool isAwake = false;
    private bool spiderCurrentlyShooting = false;
    private bool spiderHasShotFirstShot = false; 
    private bool spiderAnimationPlayed = false;
    private float spiderTimeToDeath = .8f;
    private float deathTimer = 0f;
    //difficulty
    private float reactionTime = 0f; 
    private float spiderReactionTime;  
    private float timeBetweenShots = 1f; 
    private float spiderTimeBetweenShots;
    private float unholsterTime = 0.5f; //never change, this is the time for unholster animation to play
    private float spiderAccuracy = 0.5f; //spiders accuracy range, +- this value
    private int spiderDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.Find("CharacterJustin");
        spiderPace = pacingTime;
        spiderReactionTime = reactionTime;
        spiderTimeBetweenShots = timeBetweenShots;
        DifficultyCheck();
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
        Pace();
        TargetDetect();
        SpiderShooting();
        Die();
    }

    void DifficultyCheck()
    {
        if(difficulty > 3) difficulty = 3;
        if(difficulty < 1) difficulty = 1;

        if(difficulty == 1) //normal
        {
            spiderHealth = 1;
            spiderSightDistance = 11f;
            timeBetweenShots = 0.85f;
            reactionTime = 0.25f;
            spiderAccuracy = 0.7f;
        }
        else if (difficulty == 2) //hard
        {
            spiderHealth = 1;
            spiderSightDistance = 14f;
            timeBetweenShots = 0.65f;
            reactionTime = 0.15f; 
            spiderAccuracy = 0.5f;
        }
        else if (difficulty == 3) //insane
        {
            spiderHealth = 1;
            spiderSightDistance = 17f;
            timeBetweenShots = 0.45f;
            reactionTime = 0.0f;
            spiderAccuracy = 0.3f;
        }
        else Debug.Log("SPIDER DIFFICULTY ERROR");
    }


    void SpiderShooting()
    {
        if(spiderDead || targetObject == null) return;

        //if target is in sights, spider stops walking, aims for head(or feet if head is not visable) then shoots repeatadly until target leaves sights.
        if(targetInSights)
        {
            spiderCurrentlyShooting = true;

            //BLUE debug ray (shows where spider is going to shoot)
            Vector2 direction = targetObject.transform.position - firePoint.position;
            if (aimForHead) direction.y += 1.75f; //offset to aim for chest or feet
            else direction.y += .5f;
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,spiderSightDistance,layerMask);
            Debug.DrawRay(firePoint.position,direction,Color.blue);

            //If spider is not ready to shoot, get him ready to shoot
            if(!isAwake) 
            {
            gameObject.GetComponent<Animator>().SetTrigger("SpiderAwake");
            isAwake = true;
            }

            //handles shooting
            spiderReactionTime -= Time.deltaTime;
            if(spiderReactionTime <= 0.0f){
                if(!spiderHasShotFirstShot) //if spider has not shot after being awoken, shoot
                {
                    unholsterTime -= Time.deltaTime;
                    if(unholsterTime <= 0.0f)
                    {
                        ShootBullet();
                        spiderHasShotFirstShot = true;
                    }
                }
                else //if spider has shot after being awoken, start shooting on timer (timebetweenshots)
                {
                    spiderTimeBetweenShots -= Time.deltaTime;
                    if(spiderTimeBetweenShots <= 0.0f)
                    { 
                        ShootBullet();
                        spiderTimeBetweenShots = timeBetweenShots;
                    }
                }
            }
        }
        else 
        {
            //after target leaves range, spider holsters then returns to walking.
            if(spiderCurrentlyShooting)
            {
                gameObject.GetComponent<Animator>().SetTrigger("HolsterThenWalk");
            }
            spiderCurrentlyShooting = false;
            isAwake = false;

            //after target leaves range, reset time until next bullet is shot.
            spiderTimeBetweenShots = timeBetweenShots;
            spiderReactionTime = reactionTime;
            unholsterTime = 0.5f;
            spiderHasShotFirstShot = false;
        }
    }


    void ShootBullet()
    {
        if(spiderDead || targetObject == null) return;

        gameObject.GetComponent<Animator>().Play("Spider Shoot");
        FindObjectOfType<AudioManager>().Play("shoot");
        float sign = 1;
        float offset = 0;

        //default aims for head, if head is not visable, aims for feet.
        Vector2 direction = targetObject.transform.position - firePoint.position;
        if (aimForHead) direction.y += 1.75f; 
        else direction.y += .5f;
        
        //gives a random aim offset, so spider doesn't always shoot at the exact same place
        float AimRandomizer = UnityEngine.Random.Range(-spiderAccuracy, spiderAccuracy);
        direction.y += AimRandomizer;
        
        //takes care of aiming offset for above and below the firepoint
        if (direction.y >= 0) sign = -1;
        else sign = 1;
        if (sign >= 0) offset = 0;
        else offset = 360;
        float angle = Vector2.Angle(Vector2.left, direction) * sign + offset;

        //fires bullet
        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
    }

    void TargetDetect()
    {
        if(spiderDead || targetObject == null) return;

        //if target head or feet are found, target is in sights
        if ((HeadFound() && FeetFound()) || (HeadFound() || FeetFound()))
        {
            targetInSights = true;
            return;
        }
        targetInSights = false;
    }


    bool HeadFound()
    {
        //looks for head
        Vector2 direction = targetObject.transform.position - transform.position;
        direction.y += 2f; //offset vector so spider looks for head
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,spiderSightDistance,layerMask);
        
        //if spider is facing target, and target is in range, and target's head is visible, head is found.
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((facingRight && (direction.x > 0)) || (!facingRight && (direction.x < 0))))
        {
            //Debug.DrawRay(transform.position,direction,Color.green);
            aimForHead = true;
            return true;
        }
        //else Debug.DrawRay(transform.position,direction,Color.red);
        aimForHead = false;
        return false;
    }


    bool FeetFound()
    {
        //looks for feet, slightly less spider sight distance than head
        Vector2 direction = targetObject.transform.position - transform.position;
        direction.y += .25f; //offset vector so spider looks for feet
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,spiderSightDistance - 0.1f,layerMask);
        
        //if spider is facing target, and target is in range, and target's feet are visible, feet are found.
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((facingRight && direction.x > 0) || (!facingRight && direction.x < 0)))
        {
            //Debug.DrawRay(transform.position,direction,Color.green);
            return true;
        }
        //else Debug.DrawRay(transform.position,direction,Color.red);
        return false;
    }


    void Pace()
    {
        //if spider is shooting or dead, stop movement.
        if(spiderCurrentlyShooting || spiderDead) {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0.0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);    
            return;
        }

        //when timer reaches "SpiderPace", multiply spider movement speed by -1, moving him in the opposite direction.
        if (spiderPace <= 0f)
        {
            spiderSpeedMultiplier *= -1f;
            spiderPace = pacingTime;
        }
        spiderPace -= Time.deltaTime;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (spiderSpeedMultiplier * spiderSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    //flip sprite if spider changes direction
    void FlipSprite()
    {
        if(spiderCurrentlyShooting || spiderDead) return;
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x < 0.0f && facingRight == true) 
        {
            facingRight = !facingRight;
            gameObject.transform.Rotate(0f, 180, 0f);
        }
        else if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0.0f && facingRight == false) 
        {
            facingRight = !facingRight;
            gameObject.transform.Rotate (0f, 180, 0f);
        }

        //if spider reaches wall, flip
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -gameObject.transform.right, 1.1f, groundLayerMask);
        Debug.DrawRay(transform.position, -gameObject.transform.right * 1.1f,Color.green);
        if(hitInfo.collider != null && hitInfo.collider.tag == "Ground")
        {
            //Debug.Log(hitInfo.collider.tag);
            facingRight = !facingRight;
            gameObject.transform.Rotate (0f, 180, 0f);
            spiderSpeedMultiplier *= -1f;
            spiderPace = pacingTime;
            //Debug.Log("SpiderHitWall");
        }
    }


    void OnTriggerEnter2D(Collider2D trig) 
    {
        player_controller player = trig.GetComponent<player_controller>();
        if (player != null) player.TakeDamage(spiderDamage);  
    }

    public void TakeDamage ( int damage) 
    {
        spiderHealth -= damage;
    }

    void Die()
    {
        if(spiderHealth > 0) return;
        spiderDead = true;
        FindObjectOfType<AudioManager>().Play("spider die");
        
        //Play death animation, then die.
        if(!spiderAnimationPlayed)
        {
            gameObject.GetComponent<Animator>().Play("Spider Die");
            spiderAnimationPlayed = true;

            //player gets 50 points for each spider kill
            SaveData SaveManager = GameObject.Find("SaveData").GetComponent<SaveData>();
            SaveManager.UpdateCurrentScore(SceneManager.GetActiveScene().buildIndex, 50);
        }
        deathTimer += Time.deltaTime;
            if(deathTimer > spiderTimeToDeath){
                Destroy(gameObject);
            }
    }
}
