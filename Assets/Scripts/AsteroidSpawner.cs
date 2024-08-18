using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject asteroidHolder;
    public float spawnRate = 1.0f; // Time in seconds between spawns
    public float minVelocity = 2.0f;
    public float maxVelocity = 5.0f;

    private float screenWidth;
    private float screenHeight;

    void Start()
    {
        GUI.init();
        // Convert screen size to world units
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize;

        // Start the spawning process
        StartCoroutine(SpawnAsteroids());
    }

    void Update() { }

    IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            SpawnAsteroid();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnAsteroid()
    {
        // Determine a random edge to spawn from
        GUI.updateScore(1);
        int edge = Random.Range(0, 4);
        Vector2 spawnPosition = Vector2.zero;

        switch (edge)
        {
            case 0: // Left edge
                spawnPosition = new Vector2(-screenWidth, Random.Range(-screenHeight, screenHeight));
                break;
            case 1: // Right edge
                spawnPosition = new Vector2(screenWidth, Random.Range(-screenHeight, screenHeight));
                break;
            case 2: // Top edge
                spawnPosition = new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight);
                break;
            case 3: // Bottom edge
                spawnPosition = new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight);
                break;
        }

        // Instantiate the asteroid

        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.transform.parent = asteroidHolder.transform;
        // Calculate a random inward direction
        Vector2 directionToCenter = (Vector2)Camera.main.transform.position - spawnPosition;
        directionToCenter.Normalize();

        // Add some randomness to the direction
        Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
        Vector2 finalDirection = directionToCenter + randomOffset;
        finalDirection.Normalize();

        // Apply the velocity
        Rigidbody2D asteroidRb = asteroid.GetComponent<Rigidbody2D>();
        float speed = Random.Range(minVelocity, maxVelocity);
        asteroidRb.velocity = finalDirection * speed;
    }
}
