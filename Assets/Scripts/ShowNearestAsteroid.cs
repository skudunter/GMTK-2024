using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNearestAsteroid : MonoBehaviour
{
    private GameObject player;
    private ShootGrappleGun shootGrappleGun;
    private float range;    
    void Start()
    {
        player = GameObject.Find("Player");
        shootGrappleGun = player.GetComponent<ShootGrappleGun>();
        range = shootGrappleGun.getRange();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        GameObject nearestAsteroid = null;
        float nearestDistance = Mathf.Infinity;
        foreach (GameObject asteroid in asteroids)
        {
            float distance = Vector2.Distance(player.transform.position, asteroid.transform.position);
            if (distance < nearestDistance)
            {
                nearestAsteroid = asteroid;
                nearestDistance = distance;
            }
        }
        if (nearestAsteroid != null && nearestDistance < range)
        {
        //   nearestAsteroid.GetComponent<IsNearest>().SetIsNearest(true);
        }  
    }
}
