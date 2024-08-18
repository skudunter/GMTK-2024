using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class GUI 
{
    private static GameObject scoreText ;
    public static void updateScore(float score)
    {
        scoreText = GameObject.Find("ScoreText");
        scoreText.GetComponent<TextMeshPro>().text = score.ToString();
    }
    public static void init()
    {
    }
}
