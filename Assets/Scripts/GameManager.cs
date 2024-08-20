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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
