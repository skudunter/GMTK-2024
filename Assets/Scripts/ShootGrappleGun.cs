using UnityEngine;

public class ShootGrappleGun : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private Transform grapplePoint;
    private SpringJoint2D joint;

    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private Transform firePoint;

    // Tractor beam
    [SerializeField]
    private float tractor_beam_desired_distance = 3.0f;

    private bool tractor_beam_enabled = false;
    private bool tractor_beam_caught = false;

    private float tractor_beam_time = 0.0f;

    [SerializeField]
    private float tractor_beam_force_push = 20.0f;

    [SerializeField]
    private float tractor_beam_force_pull = 10.0f;

    private GameObject player;

    private GameObject asteroidManager;
    private GameObject closestAsteroid;

    [SerializeField]
    private int ropeSegments = 10; // Number of segments for the rope

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = ropeSegments;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.enabled = false;

        player = GameObject.Find("Player");

        joint = gameObject.AddComponent<SpringJoint2D>();
        joint.enabled = false;
        asteroidManager = GameObject.Find("AsteroidManager");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootGrapple();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            ReleaseGrapple();
        }

        if (!tractor_beam_enabled)
        {
            closestAsteroid = GetClosestAsteroid();
        }

        if (closestAsteroid != null)
        {
            // play animation when asteroid is close
            Animator animator = closestAsteroid.GetComponent<Animator>();
            if (animator.GetCurrentAnimatorClipInfo(0).Length == 0)
            {
                animator.Play("ping");
            }

            // Apply tractor beam force
            if (tractor_beam_enabled)
            {
                // Set rope thickness based on held time.
                lineRenderer.startWidth = 0.15f * Mathf.Min(1.0f, tractor_beam_time / 2.0f);
                lineRenderer.endWidth = 0.15f * Mathf.Min(1.0f, tractor_beam_time / 2.0f);

                Rigidbody2D asteroidRb = closestAsteroid.GetComponent<Rigidbody2D>();
                Vector2 desiredPos =
                    player.transform.position + player.transform.up * tractor_beam_desired_distance;
                float distance = Vector2.Distance(desiredPos, asteroidRb.position);

                if (distance < 0.4)
                {
                    asteroidRb.MovePosition(desiredPos);
                    tractor_beam_caught = true;
                }
                else
                {
                    if (tractor_beam_caught)
                    {
                        asteroidRb.MovePosition(desiredPos);
                    }
                    else
                    {
                        asteroidRb.velocity =
                            (desiredPos - asteroidRb.position).normalized * tractor_beam_force_pull;
                    }
                }

                tractor_beam_time += Time.deltaTime;
                if (tractor_beam_time > 2.0f)
                {
                    ReleaseGrapple();
                }
                DrawRope();
            }
        }
        else
        {
            lineRenderer.enabled = false;
            tractor_beam_enabled = false;
            tractor_beam_caught = false;
        }
    }

    void ShootGrapple()
    {
        GameObject closestAsteroid = null;
        float closestDistance = range;

        for (int i = 0; i < asteroidManager.transform.childCount; i++)
        {
            GameObject asteroid = asteroidManager.transform.GetChild(i).gameObject;
            float distance = Vector2.Distance(firePoint.position, asteroid.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAsteroid = asteroid;
            }
        }

        if (closestAsteroid != null)
        {
            // Track interaction
            WasInteracttedWith wasInteracttedWith =
                closestAsteroid.GetComponent<WasInteracttedWith>();
            if (wasInteracttedWith != null)
            {
                wasInteracttedWith.SetWasInteracttedWith(true);
            }

            grapplePoint = closestAsteroid.transform;

            // Draw tractor beam
            lineRenderer.enabled = true;
            tractor_beam_enabled = true;
            tractor_beam_time = 0.0f;
        }
    }

    public void ReleaseGrapple()
    {
        joint.enabled = false;
        lineRenderer.enabled = false;

        if (closestAsteroid != null)
        {
            Rigidbody2D asteroidRb = closestAsteroid.GetComponent<Rigidbody2D>();

            Vector2 playerPos = player.transform.position;
            Vector2 forceVector =
                tractor_beam_force_push
                * (asteroidRb.position - playerPos).normalized
                * Mathf.Min(1.0f, tractor_beam_time / 3.0f);
            asteroidRb.velocity =
                tractor_beam_force_push
                * (asteroidRb.position - playerPos).normalized
                * Mathf.Min(1.0f, tractor_beam_time / 3.0f);

            closestAsteroid = null;
            tractor_beam_enabled = false;
            tractor_beam_caught = false;
            tractor_beam_time = 0.0f;
        }
    }

    private GameObject GetClosestAsteroid()
    {
        GameObject closestAsteroid = null;
        float closestDistance = range;

        for (int i = 0; i < asteroidManager.transform.childCount; i++)
        {
            GameObject asteroid = asteroidManager.transform.GetChild(i).gameObject;
            float distance = Vector2.Distance(firePoint.position, asteroid.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAsteroid = asteroid;
            }
        }

        return closestAsteroid;
    }

    void DrawRope()
    {
        // Set the start point of the rope
        lineRenderer.SetPosition(0, firePoint.position);

        // Calculate positions for each segment of the rope
        for (int i = 1; i < ropeSegments; i++)
        {
            float t = i / (float)(ropeSegments - 1);
            Vector3 position = Vector3.Lerp(firePoint.position, grapplePoint.position, t);
            lineRenderer.SetPosition(i, position);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
