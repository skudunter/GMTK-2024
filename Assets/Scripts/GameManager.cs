using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using TMPro;

public static class GameManager
{
    private static int score = 0;
    private static GameObject player;
    private static GameObject audioInitializer;
    private static GameObject vortex;
    private static GameObject asteroidManager;

    public static void Init()
    {
        player = GameObject.Find("Player");
        audioInitializer = GameObject.Find("AudioInitializer");
        vortex = GameObject.Find("Vortex");
        asteroidManager = GameObject.Find("AsteroidManager");
    }

    public static void KillPlayer()
    {
        player.GetComponent<Animator>().Play("asteroidDeath");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<ShootGrappleGun>().enabled = false;
        player.GetComponent<ShootLaser>().enabled = false;
        audioInitializer.GetComponent<AudioSource>().volume = 0.2f;
    }

    public static void RestartGame()
    {
        Cursor.visible = false;

        player.GetComponent<Animator>().Play("New State");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        player.GetComponent<Transform>().rotation = Quaternion.identity;
        player.GetComponent<Transform>().position = new Vector3(0, -4, 0);
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<ShootGrappleGun>().enabled = true;
        player.GetComponent<ShootLaser>().enabled = true;
        player.transform.GetChild(0).gameObject.SetActive(true);
        player.transform.GetChild(4).gameObject.SetActive(true);

        audioInitializer.GetComponent<AudioSource>().volume = 1f;

        score = 0;
        GUI.updateScore(score);
        GUI.updateLaserCharge(-4);
        GUI.HideRestartScreen();

        asteroidManager.GetComponent<AsteroidSpawner>().ResumeSpawner();

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject child in asteroids)
        {
            GameObject.Destroy(child);
        }
        vortex.GetComponent<GravitationalAttraction>().gravitationalConstant = 1f;
        vortex.GetComponent<DestroyOnContactAndScaleBehavior>().isPaused = false;
        vortex.transform.localScale = new Vector3(1, 1, 1);
    }

    public static void AddScore(int points)
    {
        SoundManager.PlayScoreSound(new Vector3(0, 0, 0));
        score += points;
        GUI.updateScore(score);
    }

    public static void KillApplication()
    {
        // kill the game
        Application.Quit();
    }

    public static void DoScreenShake(float intensity)
    {
        Camera.main.GetComponent<ScreenShake>().Shake(intensity);
    }

    static int ReadHighScore()
    {
        var score = int.Parse(File.ReadAllLines("Assets/Textfiles/highscore.txt")[0]);
        return score;
        // read a text file
    }

    public static void PauseGame()
    {
        Cursor.visible = true;
        vortex.GetComponent<GravitationalAttraction>().gravitationalConstant = 0f;
        asteroidManager.GetComponent<AsteroidSpawner>().PauseSpawner();
        player.transform.GetChild(0).gameObject.SetActive(false);
        player.transform.GetChild(4).gameObject.SetActive(false);

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject child in asteroids)
        {
            Rigidbody2D childRB = child.GetComponent<Rigidbody2D>();

            if (childRB != null)
            {
                childRB.velocity = Vector2.zero;
            }
        }
    }
}
