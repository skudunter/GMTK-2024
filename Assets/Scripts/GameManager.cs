using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    private static int score = 0;
    private static GameObject player;
    private static GameObject audioInitializer;
    private static GameObject vortex;

    public static void Init()
    {
        player = GameObject.Find("Player");
        audioInitializer = GameObject.Find("AudioInitializer");
        vortex = GameObject.Find("Vortex");
    }

    public static void KillPlayer()
    {
        player.GetComponent<Animator>().Play("asteroidDeath");

        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<ShootGrappleGun>().enabled = false;
        player.GetComponent<ShootLaser>().enabled = false;
        audioInitializer.GetComponent<AudioSource>().volume = 0.2f;
    }

    public static void RestartGame()
    {
        player.GetComponent<Animator>().Play("New State");
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0;
        player.GetComponent<Transform>().rotation = Quaternion.identity;
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<ShootGrappleGun>().enabled = true;
        player.GetComponent<ShootLaser>().enabled = true;
        audioInitializer.GetComponent<AudioSource>().volume = 1f;
        Time.timeScale = 1;
        score = 0;
        GUI.updateScore(score);
        GUI.HideRestartScreen();

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject child in asteroids)
        {
            GameObject.Destroy(child);
        }
        vortex.GetComponent<GravitationalAttraction>().gravitationalConstant = 1f;
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

    public static void ShowRestartScreen()
    {
        GUI.ShowRestartScreen();
    }
}
