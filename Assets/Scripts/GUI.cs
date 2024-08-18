using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class GUI
{
    private static GameObject scoreText = GameObject.Find("ScoreText");

    public static void updateScore(float score)
    {
        if (!scoreText)
        {
            Debug.LogError("Score text not initialized lol");
            return;
        }
        scoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

}
