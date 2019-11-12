using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : MonoBehaviour
{


    public int PacingTime = 4;
    public float SpiderSpeed = 1;
    private bool FacingRight = false;
    private float SpiderPace;
    private float SpiderSpeedMultiplier = 1;


    public float SpiderSightDistance = 12f;
    public GameObject TargetObject;
    private bool TargetInSights = false;
    private bool IsAwake = false;
    private bool SpiderCurrentlyShooting = false;
    int layerMask = ~(1 << 8); //raycast ignores all but player layer

    public GameObject bulletPrefab;
    public Transform firePoint;

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
    }

    void SpiderShooting()
    {
        if(TargetInSights)
        {
            SpiderCurrentlyShooting = true;
            if(!IsAwake) 
            {
            gameObject.GetComponent<Animator>().SetTrigger("SpiderAwake");
            IsAwake = true;
            }
            ShootBullet();
        }
        else 
        {
            if(SpiderCurrentlyShooting)
            {
                gameObject.GetComponent<Animator>().SetTrigger("HolsterThenWalk");
            }
            SpiderCurrentlyShooting = false;
            IsAwake = false;
        }

    }

    void ShootBullet()
    {
        Transform firePoint = gameObject.transform;

        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 180, 0));
        Debug.Log("fired Bullet");
    }



    void PlayerDetect()
    {
        if ((HeadFound() && FeetFound()) || (HeadFound() || FeetFound()))
        {
            TargetInSights = true;
            Debug.Log("Player compromised, spider in pursuit...");
            return;
        }
        TargetInSights = false;
    }

    bool HeadFound()
    {
        Vector2 direction = TargetObject.transform.position - transform.position;
        direction.y += 2f; //offset vector so spider looks for head
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SpiderSightDistance,layerMask);
        
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((FacingRight && (direction.x > 0)) || (!FacingRight && (direction.x < 0))))
        {
            Debug.DrawRay(transform.position,direction,Color.green);
            return true;
        }
        else Debug.DrawRay(transform.position,direction,Color.red);
        return false;
    }

    bool FeetFound()
    {
        Vector2 direction = TargetObject.transform.position - transform.position;
        direction.y += .25f; //offset vector so spider looks for feet
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SpiderSightDistance,layerMask);
        
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
        if(SpiderCurrentlyShooting) return;
        if (SpiderPace <= 0f)
        {
            SpiderSpeedMultiplier *= -1f;
            SpiderPace = PacingTime;
        }
        SpiderPace -= Time.deltaTime;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (SpiderSpeedMultiplier * SpiderSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        
    }

    //flip sprite if changes direction
    void FlipSprite()
    {
        if (gameObject.GetComponent<Rigidbody2D>().velocity.x < 0.0f && FacingRight == true) 
        {
            FacingRight = !FacingRight;
            gameObject.transform.Rotate (0f, 180, 0f);
        }
        else if (gameObject.GetComponent<Rigidbody2D>().velocity.x > 0.0f && FacingRight == false) 
        {
            FacingRight = !FacingRight;
            gameObject.transform.Rotate (0f, 180, 0f);
        }
    }

    void Die()
    {
        //play death animation
        //wait for death animation to be over
        Destroy(gameObject);
    }
}
