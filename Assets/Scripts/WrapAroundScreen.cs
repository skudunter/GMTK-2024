using UnityEngine;

public class WrapAroundScreen : MonoBehaviour
{
    private Camera mainCamera;
    private ShootGrappleGun shootGrappleGun;

    void Start()
    {
        mainCamera = Camera.main;
        shootGrappleGun = GetComponent<ShootGrappleGun>();
        if (!shootGrappleGun){
            shootGrappleGun = FindObjectOfType<ShootGrappleGun>();
        }
    }

    void Update()
    {
        DoWrapAroundScreen();
    }

    private void DoWrapAroundScreen()
    {
        Vector3 newPosition = transform.position;
        Vector3 screenBounds = mainCamera.ScreenToWorldPoint(
            new Vector3(Screen.width, Screen.height, 0)
        );

        if (transform.position.x > screenBounds.x)
        {
            newPosition.x = -screenBounds.x;
            if (shootGrappleGun)
            {
                shootGrappleGun.ReleaseGrapple();
            }
        }
        else if (transform.position.x < -screenBounds.x)
        {
            newPosition.x = screenBounds.x;
            if (shootGrappleGun)
            {
                shootGrappleGun.ReleaseGrapple();
            }
        }

        if (transform.position.y > screenBounds.y)
        {
            newPosition.y = -screenBounds.y;
            if (shootGrappleGun)
            {
                shootGrappleGun.ReleaseGrapple();
            }
        }
        else if (transform.position.y < -screenBounds.y)
        {
            newPosition.y = screenBounds.y;
            if (shootGrappleGun)
            {
                shootGrappleGun.ReleaseGrapple();
            }
        }

        transform.position = newPosition;
    }
}
