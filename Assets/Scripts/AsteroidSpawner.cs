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

    [Header("Asteroid Distribution")]
    [SerializeField] private AnimationCurve normalAsteroidCurve; // Controls distribution of normal asteroids
    [SerializeField] private AnimationCurve denseAsteroidCurve;  // Controls distribution of dense asteroids
    [SerializeField] private AnimationCurve lightAsteroidCurve;  // Controls distribution of light asteroids

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
            float adjustedSpawnRate = baseSpawnRate / spawnRateCurve.Evaluate(elapsedTime);
            SpawnAsteroid();
            yield return new WaitForSeconds(adjustedSpawnRate);
            elapsedTime += Time.deltaTime;
        }
    }

    private void SpawnAsteroid()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        GameObject asteroidPrefab = GetAsteroidPrefabBasedOnDistribution();
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        asteroid.transform.parent = transform;

        InitializeAsteroid(asteroid, spawnPosition);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        int edge = Random.Range(0, 4);

        switch (edge)
        {
            case 0: return new Vector2(-screenWidth, Random.Range(-screenHeight, screenHeight)); // Left edge
            case 1: return new Vector2(screenWidth, Random.Range(-screenHeight, screenHeight));  // Right edge
            case 2: return new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight);   // Top edge
            case 3: return new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight);  // Bottom edge
            default: return Vector2.zero; // Fallback, shouldn't occur
        }
    }

    private GameObject GetAsteroidPrefabBasedOnDistribution()
    {
        float normalWeight = normalAsteroidCurve.Evaluate(elapsedTime);
        float denseWeight = denseAsteroidCurve.Evaluate(elapsedTime);
        float lightWeight = lightAsteroidCurve.Evaluate(elapsedTime);

        float totalWeight = normalWeight + denseWeight + lightWeight;
        float randomValue = Random.Range(0, totalWeight);

        if (randomValue <= normalWeight)
        {
            return normalAsteroidPrefab;
        }
        else if (randomValue <= normalWeight + denseWeight)
        {
            return denseAsteroidPrefab;
        }
        else
        {
            return lightAsteroidPrefab;
        }
    }

    private void InitializeAsteroid(GameObject asteroid, Vector2 spawnPosition)
    {
        AsteroidStats asteroidStats = asteroid.GetComponent<AsteroidStats>();

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
        asteroid.transform.Rotate(Random.Range(0, maxSpin), Random.Range(0, maxSpin), Random.Range(0, maxSpin));
        asteroid.transform.GetChild(0).transform.localScale = new Vector3(size, size, size);
        asteroidRb.angularVelocity = Random.Range(-maxSpin, maxSpin);
        asteroidRb.mass = size * asteroidStats.density;
        asteroidRb.velocity = finalDirection * speed;
    }
}
