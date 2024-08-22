using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CustomSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject panel;
    private AudioSource audio;

    void Start()
    {
        // hide the mouse
        panel = GameObject.Find("ScreenPanel");
        if (GameObject.Find("Audio") != null)
        {
            audio = GameObject.Find("Audio").GetComponent<AudioSource>();
        }
        if (panel != null)
        {
            panel.SetActive(false);
        }

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update() { }

    public void RestartGame()
    {
        GameManager.RestartGame();
    }

    public void EndGame()
    {
        GameManager.KillApplication();
    }

    private bool inGame = false;
    private bool inCredits = false;

    public void TransitionToGame()
    {
        StartCoroutine(TransitionToGameCoroutine());
    }

    IEnumerator TransitionToGameCoroutine()
    {
        panel.SetActive(true);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel.GetComponent<Animator>().Play("FadeOut");
        audio.volume = 0.5f;

        yield return new WaitForSeconds(0.8f);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 255);
        SceneManager.LoadScene("Game");
    }

    public void TransitionToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void TransitionToCredits()
    {
        StartCoroutine(TransitionToCreditsCoroutine());
    }

    IEnumerator TransitionToCreditsCoroutine()
    {
        panel.SetActive(true);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel.GetComponent<Animator>().Play("FadeOut");
        audio.volume = 0.5f;
        yield return new WaitForSeconds(0.8f);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 255);
        SceneManager.LoadScene("Credits");
        // inGame = true;
    }

    public void TransitionToMainMenu()
    {
        Debug.Log("Transitioning to main menu");
        StartCoroutine(TransitionToMainMenuCoroutine());
    }

    IEnumerator TransitionToMainMenuCoroutine()
    {
        panel.SetActive(true);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
        panel.GetComponent<Animator>().Play("FadeOut");

        yield return new WaitForSeconds(0.8f);
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 255);
        SceneManager.LoadScene("MainMenu");
    }
}
