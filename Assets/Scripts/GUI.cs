using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class GUI
{
    public static float laserCharge = 0;
    private static GameObject scoreText = GameObject.Find("ScoreText");
    private static  GameObject laserChargeIndicator = GameObject.Find(
        "LaserChargeIndicator"
    );
    private static GameObject restartScreen = GameObject.Find("RestartMenu");
    private static GameObject highScoreText = GameObject.Find("HighScoreText");

    private static GameObject titleText = GameObject.Find("Title");

    public static void Init()
    {
        // scoreText = GameObject.Find("ScoreText");
        // laserChargeIndicator = GameObject.Find("LaserChargeIndicator");
        // restartScreen = GameObject.Find("RestartMenu");
        // highScoreText = GameObject.Find("HighScoreText");
        // titleText = GameObject.Find("Title");
        if (restartScreen != null)
        {
            restartScreen.SetActive(false);
        }
    }

    public static void UpdateTitleText(string title)
    {
        if (!titleText)
        {
            Debug.Log("Title text not initialized lol");
            return;
        }
        titleText.GetComponent<TextMeshProUGUI>().text = title;
    }

    public static void updateScore(float score)
    {
        if (!scoreText)
        {
            Debug.LogError("Score text not initialized lol");
            return;
        }
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public static void UpdateHighScore(float score)
    {
        if (highScoreText == null)
        {
            Debug.Log("Highscore text not initialized lol");
            return;
        }
        highScoreText.GetComponent<TextMeshProUGUI>().text = "Highscore: " + score.ToString();
    }

    public static void updateLaserCharge(float change)
    {
        laserCharge += change;
        if (laserCharge > 4)
        {
            laserCharge = 4;
        }
        else if (laserCharge < 0)
        {
            laserCharge = 0;
        }
        if (!laserChargeIndicator)
        {
            Debug.LogError("Laser charge indicator not initialized lol");
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            if (i < laserCharge)
            {
                laserChargeIndicator.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                laserChargeIndicator.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public static void ShowRestartScreen()
    {
        restartScreen.SetActive(true);
    }

    public static void HideRestartScreen()
    {
        restartScreen.SetActive(false);
    }
}
