using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalAttraction : MonoBehaviour
{
    [SerializeField]
    private float gravitationalConstant = 0.1f;

    private GameObject player;
    private Rigidbody2D playerRb;
    private GameObject asteroidsHolder;

    void Start()
    {
        asteroidsHolder = GameObject.Find("Asteroids");
        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody2D>();

        if (!asteroidsHolder)
        {
            Debug.LogError("Asteroids not assigned");
        }
        if (!player)
        {
            Debug.LogError("Player not assigned");
        }
        if (!playerRb)
        {
            Debug.LogError("Player Rigidbody2D not found");
        }
    }

    void Update()
    {
        ApplyGravitationalForce(playerRb);
        foreach (Transform asteroidTransform in asteroidsHolder.transform)
        {
            Rigidbody2D asteroidRb = asteroidTransform.GetComponent<Rigidbody2D>();
            if (asteroidRb != null)
            {
                ApplyGravitationalForce(asteroidRb);
            }
        }
    }

    void ApplyGravitationalForce(Rigidbody2D rb)
    {
        Vector2 directionToBlackHole = (Vector2)transform.position - rb.position;
        float distance = directionToBlackHole.magnitude;
        if (distance < 0.1f)
        {
            distance = 0.1f;
        }
        else if (distance < 20f)
        {
            directionToBlackHole.Normalize();
            float forceMagnitude = gravitationalConstant * (rb.mass * 1) / (distance * distance);
            Vector2 force = directionToBlackHole * forceMagnitude;
            rb.AddForce(force);
        }
        else{
            directionToBlackHole.Normalize();
            float forceMagnitude = gravitationalConstant * (rb.mass * 1) / 400f;
            Vector2 force = directionToBlackHole * forceMagnitude;
            rb.AddForce(force); 
        }
    }

    public void IncrementGravitationalConstant(float delta)
    {
        gravitationalConstant += delta;
    }
}
