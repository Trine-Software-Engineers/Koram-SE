using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : MonoBehaviour
{
    [Header("Skeleton Settings:")]
    [Tooltip("1 = normal, 2 = hard, 3 = insane")]
    public int difficulty = 1;

    [Tooltip("Time in seconds for skeleton to pace.")]
    public int pacingTime = 4;

    [Tooltip("Skeleton movement speed")]
    public float skeletonSpeed = 1;


    [Header("Skeleton Debug:")]
    public bool skeletonDead = false;
    [SerializeField]
    private int skeletonHealth = 3;
    [Tooltip("Assigns Automatically")]
    public GameObject targetObject;

    //movement
    private bool facingRight = false;
    private float skeletonPace;
    private float skeletonSpeedMultiplier = -1;

    //target detection
    private bool targetInRange = false;
    private bool skeletonIsHunting = false;
    private float skeletonSightDistance = 12f;
    private float distanceToTarget = Mathf.Infinity;
    int layerMask = ~(1 << 8); //raycast ignores all but player layer
    int groundLayerMask = (1 << 11); //raycast ignores all but ground layer

    //animations
    private bool skeletonCurrentlyAttacking = false;
    private bool skeletonDeathAnimationPlayed = false;
    private float skeletonTimeToDeath = .4f;
    private float deathTimer = 0f;

    //difficulty
    private float reactionTime = 0f; 
    private float skeletonReactionTime;  

    private float timeBetweenAttacks = 1f; 
    private float skeletonTimeBetweenAttacks;

    private float skeletonHuntingSpeedMultiplier;

    private bool skeletonReadyToWalkAgain = false;
    private bool skeletonReadyToHuntAgain = false;
    private float skeletonHuntAgainTime = 1f;
    private float skeletonHuntAgain = .5f;
    
    private bool skeletonWillCheckBehind = false;
    private float checkBehindDelay = .1f;

    private int skeletonDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = GameObject.Find("CharacterJustin");
        skeletonPace = pacingTime;
        skeletonReactionTime = reactionTime;
        skeletonTimeBetweenAttacks = 0f;
        DifficultyCheck();
    }

    // Update is called once per frame
    void Update()
    {
        Pace();
        TargetDetect();
        SkeletonHunting();
        SkeletonAttacking();
        Die();
        CheckBehind();
    }

    void CheckBehind()
    {
        if(!skeletonWillCheckBehind) return;
            distanceToTarget = Vector2.Distance(targetObject.transform.position, transform.position);
            if(distanceToTarget <= 3f && !skeletonIsHunting && skeletonReadyToHuntAgain)
            {
                
                checkBehindDelay -= Time.deltaTime;
                if(checkBehindDelay <= 0.0f)
                { 
                    skeletonSpeedMultiplier *= -1f;
                    FlipSprite();
                    checkBehindDelay = .1f;
                }


            }
            else
            {
                checkBehindDelay = .1f;
            }
    }


    void DifficultyCheck()
    {
        if(difficulty > 3) difficulty = 3;
        if(difficulty < 1) difficulty = 1;

        if(difficulty == 1) //normal
        {
            skeletonHealth = 1;
            skeletonSightDistance = 11f;
            timeBetweenAttacks = 1.2f;
            reactionTime = 0.4f;
            skeletonWillCheckBehind = false;
            skeletonHuntingSpeedMultiplier = 3f;
        }
        else if (difficulty == 2) //hard
        {
            skeletonHealth = 2;
            skeletonSightDistance = 14f;
            timeBetweenAttacks = 1.05f;
            reactionTime = 0.2f;
            skeletonWillCheckBehind = true;
            skeletonHuntingSpeedMultiplier = 4.5f;
        }
        else if (difficulty == 3) //insane
        {
            skeletonHealth = 3;
            skeletonSightDistance = 17f;
            timeBetweenAttacks = 0.9f;
            reactionTime = 0.0f;
            skeletonWillCheckBehind = true;
            skeletonHuntingSpeedMultiplier = 6f;
        }
        else Debug.Log("SKELETON DIFFICULTY ERROR");
    }


    void SkeletonAttacking()
    {
        if(skeletonDead || targetObject == null) return;
        
        //if target is in sights, skeleton stops walking, aims for head(or feet if head is not visable) then shoots repeatadly until target leaves sights.
        if(targetInRange)
        {
            gameObject.GetComponent<Animator>().SetBool("TargetInRange", true);
            skeletonCurrentlyAttacking = true;

            //handles attacking
            skeletonReactionTime -= Time.deltaTime;
            if(skeletonReactionTime <= 0.0f){
        
                skeletonTimeBetweenAttacks -= Time.deltaTime;
                if(skeletonTimeBetweenAttacks <= 0.0f)
                { 
                    Attack();
                    skeletonTimeBetweenAttacks = timeBetweenAttacks;
                }
            }
        }
        else 
        {
            gameObject.GetComponent<Animator>().SetBool("TargetInRange", false);
            skeletonCurrentlyAttacking = false;
            skeletonTimeBetweenAttacks = 0;
            skeletonReactionTime = reactionTime;

            skeletonHuntAgain -= Time.deltaTime;
            if(skeletonHuntAgain <= 0.0f)
            { 
                skeletonReadyToWalkAgain = true;

                gameObject.GetComponent<Animator>().SetBool("SkeletonReadyToHuntAgain", true);
                skeletonReadyToHuntAgain = true;
            }     
        }
    }


    void Attack()
    {
        if(skeletonDead || targetObject == null) return;

        //attack with random attack
        int attackNumber = UnityEngine.Random.Range(1,4);

        if(attackNumber == 1)
        {
            gameObject.GetComponent<Animator>().Play("Slash");
        } 
        else if(attackNumber == 2)
        {
            gameObject.GetComponent<Animator>().Play("Stab");
        }
        else if(attackNumber == 3)
        {
            gameObject.GetComponent<Animator>().Play("Whack");
        } 

        skeletonReadyToWalkAgain = false;

        skeletonReadyToHuntAgain = false;
        gameObject.GetComponent<Animator>().SetBool("SkeletonReadyToHuntAgain", false);
        skeletonHuntAgain = skeletonHuntAgainTime;
    }


    void SkeletonHunting()
    {
        if(skeletonIsHunting)
        {
            distanceToTarget = Vector2.Distance(targetObject.transform.position, transform.position);
            if(distanceToTarget <= 2.25f) 
            {
                targetInRange = true;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            else
            {
                targetInRange = false;
                if(skeletonReadyToHuntAgain) gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (skeletonSpeedMultiplier * (skeletonSpeed * skeletonHuntingSpeedMultiplier), gameObject.GetComponent<Rigidbody2D>().velocity.y);
                else gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            return;
        }
        else
        {
            distanceToTarget = Mathf.Infinity;
            targetInRange = false;
            return;
        }
    }


    void TargetDetect()
    {
        if(skeletonDead || targetObject == null) return;

        //if target head or feet are found, target is in sights
        if ((HeadFound() && FeetFound()) || (HeadFound() || FeetFound()))
        {
            skeletonIsHunting = true;
            gameObject.GetComponent<Animator>().SetBool("IsHunting", true);
            return;
        }
        skeletonIsHunting = false;
        gameObject.GetComponent<Animator>().SetBool("IsHunting", false);
    }


    bool HeadFound()
    {
        //looks for head
        Vector2 direction = targetObject.transform.position - transform.position;
        direction.y += 2f; //offset vector so skeleton looks for head
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,skeletonSightDistance,layerMask);
        
        //if skeleton is facing target, and target is in range, and target's head is visible, head is found.
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((facingRight && (direction.x > 0)) || (!facingRight && (direction.x < 0))))
        {
            Debug.DrawRay(transform.position,direction,Color.green);
            return true;
        }
        else Debug.DrawRay(transform.position,direction,Color.red);
        return false;
    }


    bool FeetFound()
    {
        //looks for feet, slightly less skeleton sight distance than head
        Vector2 direction = targetObject.transform.position - transform.position;
        direction.y += .25f; //offset vector so skeleton looks for feet
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,skeletonSightDistance - 0.1f,layerMask);
        
        //if skeleton is facing target, and target is in range, and target's feet are visible, feet are found.
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((facingRight && direction.x > 0) || (!facingRight && direction.x < 0)))
        {
            Debug.DrawRay(transform.position,direction,Color.green);
            return true;
        }
        else Debug.DrawRay(transform.position,direction,Color.red);
        return false;
    }


    void Pace()
    {
        //if skeleton is shooting or dead, stop movement.
        if(skeletonDead) {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0.0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);    
            return;
        }

        if(skeletonIsHunting || skeletonCurrentlyAttacking || !skeletonReadyToWalkAgain) return;

        //if skeleton reaches wall, flip
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -gameObject.transform.right, 1f, groundLayerMask);
        if(hitInfo.collider != null && hitInfo.collider.tag == "Ground")
        {
            facingRight = !facingRight;
            gameObject.transform.Rotate (0f, 180, 0f);
            skeletonSpeedMultiplier *= -1f;
            skeletonPace = pacingTime;
        }

        //when timer reaches "SpiderPace", multiply skeleton movement speed by -1, moving him in the opposite direction.
        if (skeletonPace <= 0f)
        {
            skeletonSpeedMultiplier *= -1f;
            skeletonPace = pacingTime;
            FlipSprite();
        }
        skeletonPace -= Time.deltaTime;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (skeletonSpeedMultiplier * skeletonSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
    }

    //flip sprite if skeleton changes direction
    void FlipSprite()
    {
        //if skeleton is at the end of pace, flip
        if(skeletonCurrentlyAttacking || skeletonDead) return;

        facingRight = !facingRight;
        gameObject.transform.Rotate(0f, 180, 0f);
        
    }


    void OnTriggerEnter2D(Collider2D trig) 
    {
        player_controller player = trig.GetComponent<player_controller>();
        if (player != null) player.TakeDamage(skeletonDamage);   
    }

    public void TakeDamage ( int damage) 
    {
        skeletonHealth -= damage;
    }

    void Die()
    {
        if(skeletonHealth > 0) return;
        skeletonDead = true;
        
        //Play death animation, then die.
        if(!skeletonDeathAnimationPlayed)
        {
            gameObject.GetComponent<Animator>().Play("Die_Slower");
            skeletonDeathAnimationPlayed = true;
        }
        deathTimer += Time.deltaTime;
        if(deathTimer > skeletonTimeToDeath) Destroy(gameObject);
    }
}
