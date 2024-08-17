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
    private LayerMask asteroidLayer;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private int rayCount = 36;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
        lineRenderer.enabled = false;

        joint = gameObject.AddComponent<SpringJoint2D>();
        joint.enabled = false;
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
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grapplePoint.position);
        }
    }

    void ShootGrapple()
    {
        float angleStep = 360f / rayCount;
        RaycastHit2D closestHit = new RaycastHit2D();
        float closestDistance = range;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = i * angleStep;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up;
            RaycastHit2D hit = Physics2D.Raycast(
                firePoint.position,
                direction,
                range,
                asteroidLayer
            );

            if (hit.collider != null && hit.distance < closestDistance)
            {
                closestHit = hit;
                closestDistance = hit.distance;
            }
        }

        if (closestHit.collider != null)
        {
            grapplePoint = closestHit.transform;
            joint.connectedBody = closestHit.rigidbody;
            joint.autoConfigureDistance = false;
            joint.distance = closestDistance;
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
