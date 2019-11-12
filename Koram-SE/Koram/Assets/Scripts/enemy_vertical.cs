using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_vertical : MonoBehaviour
{

    public float speed;

    public int damage;
    
    public bool movingUp = true;

    public Transform terrainDetection;

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
    }


}
