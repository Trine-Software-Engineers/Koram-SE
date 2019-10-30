using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_vertical : MonoBehaviour
{

    public float speed;

    public int damage;

    public float projectile_speed;

    public int projectile_timer;
    
    public bool movingUp = true;

    public Transform terrainDetection;

    public Transform projectile;

    int projectile_time;

    /*void Start(){
        projectile_time = 0;
    }*/

    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        RaycastHit2D terrainInfo = Physics2D.Raycast(terrainDetection.position, Vector2.right, 1f);
        if(terrainInfo.collider == true) {
            if(movingUp == true) {
                transform.eulerAngles = new Vector3(-180, 0, 0);
                movingUp = false;
            } else {
                transform.eulerAngles = new Vector3(0,0,0);
                movingUp = true;
            }
        }

        /*if (projectile_time == projectile_timer ) {
            projectile.transform.Translate(Vector2.left * projectile_speed * Time.deltaTime);
            //detach from parent object?
            //die after period of time?
            projectile_time = projectile_timer;
        } else {
            projectile_time++;
        }*/
    }


}
