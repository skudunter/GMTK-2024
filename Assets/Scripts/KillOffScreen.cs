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
        if (transform.position.x > screenBounds.x - 2 || transform.position.x < -screenBounds.x -2 || transform.position.y > screenBounds.y + 2 || transform.position.y < -screenBounds.y -2)
        {
            Destroy(gameObject);
        }   
    }
}
