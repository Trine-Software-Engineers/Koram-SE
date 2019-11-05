using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpider : MonoBehaviour
{


    public int PacingTime = 2;
    public float SpiderSpeed = 5;
    private bool FacingRight = false;
    private float SpiderPace;
    private float SpiderSpeedMultiplier = 1;

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
    }

    void Pace()
    {
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
