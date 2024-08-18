using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOffScreen : MonoBehaviour
{
   // TODO madybe incremetn score here

    void Update()
    {
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );
        if (transform.position.x > screenBounds.x + 1 || transform.position.x < -screenBounds.x -1 || transform.position.y > screenBounds.y + 1 || transform.position.y < -screenBounds.y -1)
        {
            Destroy(gameObject);
        }   
    }
}
