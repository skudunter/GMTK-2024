using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Restart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // hide the mouse
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RestartGame()
    {
        GameManager.RestartGame();
    }
    public void EndGame()
    {
       GameManager.KillApplication();
    }
}
