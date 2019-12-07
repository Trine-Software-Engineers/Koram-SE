using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private GameObject player;
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;

    // The player is found within the scene to focus on
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    
    }

    // This sets up the adjustable size of the camera when it is focused on the player
    // It also can lock a max or min height or length that the camera can travel in a level
    void Update()
    {
        if(player != null)
        {
            float x = Mathf.Clamp (player.transform.position.x, xMin, xMax);
            float y = Mathf.Clamp (player.transform.position.y, yMin, yMax);
            gameObject.transform.position = new Vector3 (x, y, gameObject.transform.position.z);
        }
        
    }
}
