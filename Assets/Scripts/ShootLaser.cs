using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject laserFirePoint;

    void Start()
    {
        laserFirePoint = GameObject.Find("LaserFirePoint");
        lineRenderer = laserFirePoint.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootLaserBeam();
        }
    }

    void ShootLaserBeam()
    {
        // Define the maximum laser distance
        float maxLaserDistance = 100f;

        // Perform the raycast, but now using RaycastAll to get all hits
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            laserFirePoint.transform.position,
            laserFirePoint.transform.forward,
            maxLaserDistance
        );

        // Initialize the LineRenderer if it's not already initialized
        if (lineRenderer == null)
        {
            lineRenderer = laserFirePoint.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }

        // Set the start position of the laser
        lineRenderer.SetPosition(0, laserFirePoint.transform.position);

        // If there are hits, set the end position to the farthest hit point
        if (hits.Length > 0)
        {
            Vector2 lastHitPoint = hits[hits.Length - 1].point;
            lineRenderer.SetPosition(1, lastHitPoint);

            // Process each hit (e.g., damage enemies)
            foreach (RaycastHit2D hit in hits)
            {
                if (
                    hit.collider != null
                    && hit.collider.gameObject.layer == LayerMask.NameToLayer("Asteroids")
                )
                {
                    Debug.Log("Hit: " + hit.collider.name);
                    // Add your code to damage the enemy or handle the hit here
                }
            }
        }
        else
        {
            // If no hits, set the end position to the maximum distance
            lineRenderer.SetPosition(
                1,
                laserFirePoint.transform.position
                    + laserFirePoint.transform.forward * maxLaserDistance
            );
        }
    }
}
