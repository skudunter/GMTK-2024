using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class GUI
{
    public static float laserCharge = 0;
    private static GameObject scoreText = GameObject.Find("ScoreText");
    private static readonly GameObject laserChargeIndicator = GameObject.Find(
        "LaserChargeIndicator"
    );
    private static GameObject restartScreen = GameObject.Find("Restart");
    private static GameObject highScoreText = restartScreen.transform.GetChild(1).gameObject;

    private static GameObject titleText = restartScreen.transform.GetChild(0).gameObject;

    public static void Init()
    {
        if (restartScreen != null)
        {
            restartScreen.SetActive(false);
        }
    }

    public static void UpdateTitleText(string title)
    {
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
