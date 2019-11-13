using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton1 : MonoBehaviour
{
    public int PacingTime = 4;
    public float SkeletonSpeed = 1;
    private bool FacingRight = false;
    private float SkeletonPace;
    private float SkeletonSpeedMultiplier = 1;


    public float SkeletonSightDistance = 12f;
    public GameObject TargetObject;
    private bool TargetInSights = false;
    private bool IsAwake = false;
    private bool SkeletonCurrentlyAttacking = false;
    int layerMask = ~(1 << 8); //raycast ignores all but player layer
    private float timer = 0f;
    private float waitTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FlipSprite();
        Pace();
        PlayerDetect();
        SkeletonAttacking();
    }

    void Pace()
    {
        if(SkeletonCurrentlyAttacking) {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (0.0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);    
            return;
        }
        if (SkeletonPace <= 0f)
        {
            SkeletonSpeedMultiplier *= -1f;
            SkeletonPace = PacingTime;
        }
        SkeletonPace -= Time.deltaTime;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2 (SkeletonSpeedMultiplier * SkeletonSpeed, gameObject.GetComponent<Rigidbody2D>().velocity.y);
        
    }

    void FlipSprite()
    {
        if(SkeletonCurrentlyAttacking) return;
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

        void PlayerDetect()
    {
        if ((HeadFound() && FeetFound()) || (HeadFound() || FeetFound()))
        {
            TargetInSights = true;
            Debug.Log("Player compromised, skeleton in pursuit...");
            return;
        }
        TargetInSights = false;
    }

    bool HeadFound()
    {
        Vector2 direction = TargetObject.transform.position - transform.position;
        direction.y += 2f; //offset vector so skeleton looks for head
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SkeletonSightDistance,layerMask);
        
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
        direction.y += .25f; //offset vector so skeleton looks for feet
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position,direction,SkeletonSightDistance,layerMask);
        
        if((hitInfo.collider != null && hitInfo.collider.tag == "Player") && ((FacingRight && direction.x > 0) || (!FacingRight && direction.x < 0)))
        {
            Debug.DrawRay(transform.position,direction,Color.green);
            return true;
        }
        else Debug.DrawRay(transform.position,direction,Color.red);
        return false;
    }

    void SkeletonAttacking()
    {
        if(TargetInSights)
        {
            SkeletonCurrentlyAttacking = true;
            if(!IsAwake) 
            {
            gameObject.GetComponent<Animator>().SetTrigger("SkeletonAttack");
            gameObject.GetComponent<Animator>().Play("attack");
            }
        }
    }
}
