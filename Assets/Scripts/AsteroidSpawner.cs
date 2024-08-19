using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroid Prefabs")]
    [SerializeField] private GameObject normalAsteroidPrefab;
    [SerializeField] private GameObject denseAsteroidPrefab;
    [SerializeField] private GameObject lightAsteroidPrefab;
    [SerializeField] private float maxSpin = 100.0f;

    [Header("Spawn Settings")]
    [SerializeField] private float baseSpawnRate = 1.0f; // Base time in seconds between spawns
    [SerializeField] private AnimationCurve spawnRateCurve; // Non-linear control for spawn rate

    private float screenWidth;
    private float screenHeight;
    private float elapsedTime = 0f;

    private void Start()
    {
        InitializeScreenDimensions();
        StartCoroutine(SpawnAsteroids());
    }

    private void InitializeScreenDimensions()
    {
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect;
        screenHeight = Camera.main.orthographicSize;
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            float adjustedSpawnRate = baseSpawnRate * spawnRateCurve.Evaluate(elapsedTime);
            SpawnAsteroid();
            yield return new WaitForSeconds(adjustedSpawnRate);
            elapsedTime += adjustedSpawnRate;
        }
    }

    private void SpawnAsteroid()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject asteroidPrefab = GetRandomAsteroidPrefab();
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.transform.parent = transform;

        InitializeAsteroid(asteroid, spawnPosition);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0: // Left edge
                return new Vector2(-screenWidth, Random.Range(-screenHeight, screenHeight));
            case 1: // Right edge
                return new Vector2(screenWidth, Random.Range(-screenHeight, screenHeight));
            case 2: // Top edge
                return new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight);
            case 3: // Bottom edge
                return new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight);
            default:
                return Vector2.zero; // Fallback, shouldn't occur
        }
    }

    private GameObject GetRandomAsteroidPrefab()
    {
        int rand = Random.Range(0, 3);

        switch (rand)
        {
            case 0:
                return normalAsteroidPrefab;
            case 1:
                return denseAsteroidPrefab;
            case 2:
                return lightAsteroidPrefab;
            default:
                return null; // Fallback, shouldn't occur
        }
    }

    private void InitializeAsteroid(GameObject asteroid, Vector2 spawnPosition)
    {
        AsteroidStats asteroidStats = asteroid.GetComponent<AsteroidStats>();

        // Calculate a random inward direction with some randomness
        Vector2 finalDirection = CalculateDirectionToCenter(spawnPosition);
        Rigidbody2D asteroidRb = asteroid.GetComponent<Rigidbody2D>();

        ApplyAsteroidPhysics(asteroid, asteroidRb, asteroidStats, finalDirection);
    }

    private Vector2 CalculateDirectionToCenter(Vector2 spawnPosition)
    {
        Vector2 directionToCenter = (Vector2)Camera.main.transform.position - spawnPosition;
        directionToCenter.Normalize();

        Vector2 randomOffset = new Vector2(Random.Range(-0.5f, 0.6f), Random.Range(-0.5f, 0.6f));
        Vector2 finalDirection = directionToCenter + randomOffset;
        finalDirection.Normalize();

        return finalDirection;
    }

    private void ApplyAsteroidPhysics(GameObject asteroid, Rigidbody2D asteroidRb, AsteroidStats asteroidStats, Vector2 finalDirection)
    {
        float speed = Random.Range(asteroidStats.minVelocity, asteroidStats.maxVelocity);
        float size = Random.Range(asteroidStats.minSize, asteroidStats.maxSize);
        asteroid.transform.Rotate(Random.Range(0,maxSpin),Random.Range(0,maxSpin), Random.Range(0, maxSpin));
        asteroid.transform.GetChild(0).transform.localScale = new Vector3(size, size, size);
        asteroidRb.mass = size * asteroidStats.density;
        asteroidRb.velocity = finalDirection * speed;
    }
}
