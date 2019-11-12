using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    private int count = 0;
    public int timer;
    // Update is called once per frame
    void Update()
    {
        count++;
        if (count >= timer){
            Shoot();
            count = 0;
        }
    }

    void Shoot(){
        Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 180, 0));
    }
}
