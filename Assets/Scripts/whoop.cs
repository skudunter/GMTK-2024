using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Whoop : MonoBehaviour
{
    public void EnterGame(){
        Debug.Log("entered  game");
        SceneManager.LoadScene("Game");
    }
}
