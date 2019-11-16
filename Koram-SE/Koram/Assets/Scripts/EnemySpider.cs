using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : MonoBehaviour
{
    public int PacingTime = 4;
    public float SpiderSpeed = 1;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float SpiderSightDistance = 12f;
    public GameObject TargetObject;
    public bool SpiderDead = false;


    private bool FacingRight = false;
    private float SpiderPace;
    private float SpiderSpeedMultiplier = 1;

    private bool TargetInSights = false;
    private bool IsAwake = false;
    private bool SpiderCurrentlyShooting = false;

    int layerMask = ~(1 << 8); //raycast ignores all but player layer

    private float timer = 0f;
    private float waitTime = 1f;

    private bool aimForHead = false;

    private bool SpiderAnimationPlayed = false;
    private float SpiderTimeToDeath = .8f;
    private float DeathTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        SpiderPace = PacingTime;
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
        Pace();
        PlayerDetect();
        SpiderShooting();
        Die();
    }


    void SpiderShooting()
    {
        if(SpiderDead) return;

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

            timer += Time.deltaTime;
            if(timer > waitTime){
                timer = 0f;
                gameObject.GetComponent<Animator>().Play("Spider Shoot");
                ShootBullet();
                waitTime = 0.8f;
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
            waitTime = 1f;
            timer = 0f;
        }
    }


    void ShootBullet()
    {
        if(SpiderDead) return;

        //finds angle to shoot at, from spider firepoint, to player.
        Vector2 direction = TargetObject.transform.position - firePoint.position;
        if (aimForHead) direction.y += 1.75f; //default aims for head, if head is not visable, aims for feet.
        else direction.y += .5f;
        float angle = Vector3.Angle(direction, firePoint.right);
        
        //spawns bullet prefab
        if(FacingRight) 
        {
            angle = angle + 180f;
            angle = -angle;
            Debug.Log(angle);
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        }
        else
        {
            angle = angle + 180f;
            Debug.Log(angle);
            Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
        }

    }


    void PlayerDetect()
    {
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
        //looks for feet
        Vector2 direction = TargetObject.transform.position - transform.position;
        direction.y += .25f; //offset vector so spider looks for feet
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SpiderSightDistance,layerMask);
        
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
            firePoint.transform.Rotate(0f, 180, 0f);
        }
        else if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0.0f && FacingRight == false) 
        {
            FacingRight = !FacingRight;
            gameObject.transform.Rotate (0f, 180, 0f);
            firePoint.transform.Rotate(0f, 180, 0f);
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
