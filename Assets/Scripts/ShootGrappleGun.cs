using UnityEngine;

public class ShootGrappleGun : MonoBehaviour
{
    private LineRenderer lineRenderer;

    [SerializeField]
    private float frequency = 1.5f;

    [SerializeField]
    private float dampingRatio = 0.7f;
    private Transform grapplePoint;
    private SpringJoint2D joint;

    [SerializeField]
    private float range = 100f;

    [SerializeField]
    private Transform firePoint;

    private GameObject asteroidManager;

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

        if (joint.enabled)
        {
            DrawRope();
        }

        // play animation when asteroid is close

        GameObject closestAsteroid = GetClosestAsteroid();

        if (closestAsteroid != null)
        {
            Animator animator = closestAsteroid.GetComponent<Animator>();
            if (animator.GetCurrentAnimatorClipInfo(0).Length == 0)
            {
               animator.Play("ping");
            }
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
            if (closestDistance < 1f)
            {
                joint.distance = range / 2;
                joint.autoConfigureDistance = false;
            }
            else
            {
                joint.autoConfigureDistance = true;
            }

            WasInteracttedWith wasInteracttedWith =
                closestAsteroid.GetComponent<WasInteracttedWith>();
            if (wasInteracttedWith != null)
            {
                wasInteracttedWith.SetWasInteracttedWith(true);
            }

            grapplePoint = closestAsteroid.transform;
            joint.connectedBody = closestAsteroid.GetComponent<Rigidbody2D>();
            joint.frequency = frequency;
            joint.dampingRatio = dampingRatio;
            joint.enabled = true;

            lineRenderer.enabled = true;
        }
    }

    public void ReleaseGrapple()
    {
        joint.enabled = false;
        lineRenderer.enabled = false;
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
