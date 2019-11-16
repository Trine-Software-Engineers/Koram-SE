using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : MonoBehaviour
{
    public int Difficulty = 1; // 1 = normal, 2 = hard, 3 = insane
    public int PacingTime = 4;
    public float SpiderSpeed = 1;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject TargetObject;
    public bool SpiderDead = false;

    private bool FacingRight = false;
    private float SpiderPace;
    private float SpiderSpeedMultiplier = 1;

    private bool TargetInSights = false;
    private float SpiderSightDistance = 12f;
    private bool IsAwake = false;
    private bool SpiderCurrentlyShooting = false;

    int layerMask = ~(1 << 8); //raycast ignores all but player layer
    
    private float ReactionTime = 0f; //change this in difficulty function
    private float SpiderReactionTime;  
    private float TimeBetweenShots = 1f; //change this in difficulty function
    private float SpiderTimeBetweenShots;
    private bool SpiderHasShotFirstShot = false; 
    private float UnholsterTime = 0.5f; //never change, this is the time for unholster animation to play
    private float SpiderAccuracy = 0.5f; //spiders accuracy range, +- this value

    private bool aimForHead = false;

    private bool SpiderAnimationPlayed = false;
    private float SpiderTimeToDeath = .8f;
    private float DeathTimer = 0f;

    public bool TargetInMeleeDistance = false;
    public float MeleeAnimationTime = 1f;
    private float SpiderMeleeAnimationTime;

    // Start is called before the first frame update
    void Start()
    {
        SpiderPace = PacingTime;
        SpiderReactionTime = ReactionTime;
        SpiderTimeBetweenShots = TimeBetweenShots;
        SpiderMeleeAnimationTime = MeleeAnimationTime;
    }

    // Update is called once per frame
    void Update()
    {
        DifficultyCheck();
        FlipSprite();
        Pace();
        //TargetMeleeDetect();
        //SpiderMelee();
        TargetDetect();
        SpiderShooting();
        Die();
    }

    void DifficultyCheck()
    {
        if(Difficulty > 3) Difficulty = 3;
        if(Difficulty < 1) Difficulty = 1;

        if(Difficulty == 1) //normal
        {
            SpiderSightDistance = 11f;
            TimeBetweenShots = 0.85f;
            ReactionTime = 0.25f;
            SpiderAccuracy = 0.9f;
        }
        else if (Difficulty == 2) //hard
        {
            SpiderSightDistance = 14f;
            TimeBetweenShots = 0.65f;
            ReactionTime = 0.15f;
            SpiderAccuracy = 0.6f;
        }
        else if (Difficulty == 3) //insane
        {
            SpiderSightDistance = 17f;
            TimeBetweenShots = 0.45f;
            ReactionTime = 0.0f;
            SpiderAccuracy = 0.3f;
        }
        else Debug.Log("SPIDER DIFFICULTY ERROR");
    }

    void SpiderMelee()
    {
        if(SpiderDead || TargetObject == null) return;
        
        if(TargetInMeleeDistance)
        {
            gameObject.GetComponent<Animator>().SetBool("SpiderMeleeRange", true);
            
            SpiderMeleeAnimationTime -= Time.deltaTime;
            if(SpiderMeleeAnimationTime <= 0.0f)
            {
                Debug.Log("melee");
                Melee();
                SpiderMeleeAnimationTime = MeleeAnimationTime;
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().SetBool("SpiderMeleeRange", false);
            SpiderMeleeAnimationTime = MeleeAnimationTime;
        }

    }

    void SpiderShooting()
    {
        if(SpiderDead || TargetObject == null || TargetInMeleeDistance) return;
        

        //if target is in sights, spider stops walking, aims for head(or feet if head is not visable) then shoots repeatadly until target leaves sights.
        if(TargetInSights)
        {
            SpiderCurrentlyShooting = true;



            //BLUE debug ray (shows where spider is going to shoot)
            Vector2 direction = TargetObject.transform.position - firePoint.position;
            if (aimForHead) direction.y += 1.75f; //offset to aim for chest or feet
            else direction.y += .5f;
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SpiderSightDistance,layerMask);
            Debug.DrawRay(firePoint.position,direction,Color.blue);

            //If spider is not ready to shoot, get him ready to shoot
            if(!IsAwake) 
            {
            gameObject.GetComponent<Animator>().SetTrigger("SpiderAwake");
            IsAwake = true;
            }

            //handles shooting
            SpiderReactionTime -= Time.deltaTime;
            if(SpiderReactionTime <= 0.0f){
                if(!SpiderHasShotFirstShot) //if spider has not shot after being awoken, shoot
                {
                    UnholsterTime -= Time.deltaTime;
                    if(UnholsterTime <= 0.0f)
                    {
                        ShootBullet();
                        SpiderHasShotFirstShot = true;
                    }
                }
                else //if spider has shot after being awoken, start shooting on timer (timebetweenshots)
                {
                    SpiderTimeBetweenShots -= Time.deltaTime;
                    if(SpiderTimeBetweenShots <= 0.0f)
                    { 
                        ShootBullet();
                        SpiderTimeBetweenShots = TimeBetweenShots;
                    }
                }

                
            }
        }
        else 
        {
            //after target leaves range, spider holsters then returns to walking.
            if(SpiderCurrentlyShooting)
            {
                gameObject.GetComponent<Animator>().SetTrigger("HolsterThenWalk");
            }
            SpiderCurrentlyShooting = false;
            IsAwake = false;

            //after target leaves range, reset time until next bullet is shot.
            SpiderTimeBetweenShots = TimeBetweenShots;
            SpiderReactionTime = ReactionTime;
            UnholsterTime = 0.5f;
            SpiderHasShotFirstShot = false;
        }
    }


    void ShootBullet()
    {
        if(SpiderDead || TargetObject == null) return;

        gameObject.GetComponent<Animator>().Play("Spider Shoot");
        float sign = 1;
        float offset = 0;

        //default aims for head, if head is not visable, aims for feet.
        Vector2 direction = TargetObject.transform.position - firePoint.position;
        if (aimForHead) direction.y += 1.75f; 
        else direction.y += .5f;
        
        //gives a random aim offset, so spider doesn't always shoot at the exact same place
        
        float AimRandomizer = UnityEngine.Random.Range(-SpiderAccuracy, SpiderAccuracy);
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

    void Melee()
    {
        gameObject.GetComponent<Animator>().Play("Spider Melee");
    }

    void TargetMeleeDetect()
    {
        if(SpiderDead || TargetObject == null) return;

        //looks for player
        Vector2 direction = TargetObject.transform.position - transform.position;
        direction.y += 1f; //offset vector so spider looks for chest
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, direction, 4.0f, layerMask);
        
        //if spider is facing target, and target is in range, and target's chest is visible, attack
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player")) //&& ((FacingRight && (direction.x > 0)) || (!FacingRight && (direction.x < 0))))
        {
            Debug.DrawRay(transform.position,direction,Color.cyan);
            TargetInMeleeDistance = true;
        }
        else TargetInMeleeDistance = false;
        
    }


    void TargetDetect()
    {
        if(SpiderDead || TargetObject == null || TargetInMeleeDistance) return;

        //if target head or feet are found, target is in sights
        if ((HeadFound() && FeetFound()) || (HeadFound() || FeetFound()))
        {
            TargetInSights = true;
            return;
        }
        TargetInSights = false;
    }


    bool HeadFound()
    {
        //looks for head
        Vector2 direction = TargetObject.transform.position - transform.position;
        direction.y += 2f; //offset vector so spider looks for head
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SpiderSightDistance,layerMask);
        
        //if spider is facing target, and target is in range, and target's head is visible, head is found.
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((FacingRight && (direction.x > 0)) || (!FacingRight && (direction.x < 0))))
        {
            Debug.DrawRay(transform.position,direction,Color.green);
            aimForHead = true;
            return true;
        }
        else Debug.DrawRay(transform.position,direction,Color.red);
        aimForHead = false;
        return false;
    }


    bool FeetFound()
    {
        //looks for feet, slightly less spider sight distance than head
        Vector2 direction = TargetObject.transform.position - transform.position;
        direction.y += .25f; //offset vector so spider looks for feet
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SpiderSightDistance - 0.1f,layerMask);
        
        //if spider is facing target, and target is in range, and target's feet are visible, feet are found.
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((FacingRight && direction.x > 0) || (!FacingRight && direction.x < 0)))
        {
            Debug.DrawRay(transform.position,direction,Color.green);
            return true;
        }
        else Debug.DrawRay(transform.position,direction,Color.red);
        return false;
    }


    void Pace()
    {
        //if spider is shooting or dead, stop movement.
        if(SpiderCurrentlyShooting || SpiderDead) {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0.0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);    
            return;
        }

        //when timer reaches "SpiderPace", multiply spider movement speed by -1, moving him in the opposite direction.
        if (SpiderPace <= 0f)
        {
            SpiderSpeedMultiplier *= -1f;
            SpiderPace = PacingTime;
        }
        SpiderPace -= Time.deltaTime;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (SpiderSpeedMultiplier * SpiderSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        
    }

    //flip sprite if spider changes direction
    void FlipSprite()
    {
        if(SpiderCurrentlyShooting || SpiderDead) return;
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x < 0.0f && FacingRight == true) 
        {
            FacingRight = !FacingRight;
            gameObject.transform.Rotate(0f, 180, 0f);
        }
        else if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0.0f && FacingRight == false) 
        {
            FacingRight = !FacingRight;
            gameObject.transform.Rotate (0f, 180, 0f);
        }
    }


    void Die()
    {
        if(!SpiderDead) return;

        //Play death animation, then die.
        if(!SpiderAnimationPlayed)
        {
            gameObject.GetComponent<Animator>().Play("Spider Die");
            SpiderAnimationPlayed = true;
        }
        DeathTimer += Time.deltaTime;
            if(DeathTimer > SpiderTimeToDeath){
                Destroy(gameObject);
            }
    }
}
