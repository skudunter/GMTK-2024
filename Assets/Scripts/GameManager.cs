using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    private static int score = 0;

    public static void AddScore(int points)
    {
        SoundManager.PlayScoreSound(new Vector3(0, 0, 0));
        score += points;
        GUI.updateScore(score);
    }

    public static void RestartGame()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = new Vector3(0, 5, 0);
        player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<ShootGrappleGun>().enabled = true;
        player.GetComponent<ShootLaser>().enabled = true;
        player.GetComponent<Animator>().Play("New State");
        Time.timeScale = 1;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        score = 0;
        GUI.updateScore(-4);
        GUI.HideRestartScreen();

        GameObject asteroid = GameObject.Find("AsteroidManager");

        foreach (Transform child in asteroid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        GameObject vortex = GameObject.Find("Vortex");
        vortex.GetComponent<GravitationalAttraction>().gravitationalConstant = 1f;
        vortex.transform.localScale = new Vector3(1, 1, 1);


    }
    public static void endGame()
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
