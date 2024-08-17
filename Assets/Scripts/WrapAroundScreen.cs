using UnityEngine;

public class WrapAroundScreen : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        DoWrapAroundScreen();
    }

    private void DoWrapAroundScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        if (transform.position.x > screenBounds.x)
        {
            newPosition.x = -screenBounds.x;
        }
        else if (transform.position.x < -screenBounds.x)
        {
            newPosition.x = screenBounds.x;
        }

        if (transform.position.y > screenBounds.y)
        {
            newPosition.y = -screenBounds.y;
        }
        else if (transform.position.y < -screenBounds.y)
        {
            newPosition.y = screenBounds.y;
        }

        transform.position = newPosition;
    }
}
