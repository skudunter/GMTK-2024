using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject laserFirePoint;

    [SerializeField]
    private GameObject asteroidDeathParticles;

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
            if (GUI.laserCharge == 4)
            {
                GUI.updateLaserCharge(-4);
                ShootLaserBeam();
            }
        }
    }

    void ShootLaserBeam()
    {
        SoundManager.PlayLaserSound(laserFirePoint.transform.position);
        // Define the maximum laser distance
        float maxLaserDistance = 200f;

        // Perform the raycast, but now using RaycastAll to get all hits
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            laserFirePoint.transform.position,
            laserFirePoint.transform.up,
            maxLaserDistance,
            LayerMask.GetMask("Asteroids")
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
            // Process each hit (e.g., damage enemies)
            foreach (RaycastHit2D hit in hits)
            {
                if (
                    hit.collider != null
                    && hit.collider.gameObject.layer == LayerMask.NameToLayer("Asteroids")
                    && hit.collider.gameObject.name == "asteroid"
                )
                {
                    SoundManager.PlayExplosionSound(hit.point);
                    GameObject particles = Instantiate(
                        asteroidDeathParticles,
                        hit.collider.gameObject.transform.position,
                        Quaternion.identity
                    );
                    particles.transform.GetChild(0).localScale = new Vector3(
                        hit.collider.gameObject.transform.localScale.x,
                        hit.collider.gameObject.transform.localScale.y,
                        1
                    );
                    particles.GetComponentInChildren<ParticleSystem>().GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<MeshRenderer>().material;
                    Destroy(hit.collider.gameObject.transform.parent.gameObject);
                    Destroy(particles, 1.8f);
                    GameManager.AddScore(1);
                }
            }
        }

        lineRenderer.SetPosition(
            1,
            laserFirePoint.transform.position + laserFirePoint.transform.up * maxLaserDistance
        );
        StartCoroutine(StopLaserBeam());
    }

    IEnumerator StopLaserBeam()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(lineRenderer);
    }
}
