using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class whoop : MonoBehaviour
{
    public void enterGame(){
        SceneManager.LoadScene("Game");
    }
}
