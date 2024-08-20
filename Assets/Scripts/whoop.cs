using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class whoop : MonoBehaviour
{
    public void enterGame(){
        Debug.Log("entered  game");
        SceneManager.LoadScene("Game");
    }
}
