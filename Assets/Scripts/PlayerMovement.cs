using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 180f;

    [SerializeField]
    private float thrustPower = 10f;

    [SerializeField]
    private float drag = 0.99f;

    [SerializeField]
    private float tiltAmount = 15f; // Amount of tilt when turning

    [SerializeField]
    private float tiltSmoothSpeed = 5f; // Smoothing speed for tilt

    private Rigidbody2D rb;
    private float targetTiltX = 0f;
    private float currentTiltX = 0f;
    private float targetTiltY = 0f;
    private float currentTiltY = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0f;
        rb.angularDrag = 0f;
    }

    void Update()
    {
        // Handle rotation input
        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward, -rotationInput * rotationSpeed * Time.deltaTime);

        // Calculate target tilt angles based on input
        targetTiltX = rotationInput * tiltAmount; // Tilt around x-axis when turning left/right
        targetTiltY = -rotationInput * tiltAmount / 2; // Slight tilt around y-axis for dynamic effect

        // Smoothly interpolate current tilt angles towards target tilt angles
        currentTiltX = Mathf.Lerp(currentTiltX, targetTiltX, tiltSmoothSpeed * Time.deltaTime);
        currentTiltY = Mathf.Lerp(currentTiltY, targetTiltY, tiltSmoothSpeed * Time.deltaTime);

        // Apply tilt rotation
        transform.rotation = Quaternion.Euler(currentTiltX, currentTiltY, transform.eulerAngles.z);

        // Handle thrust
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.up * thrustPower * Time.deltaTime, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * thrustPower / 3 * Time.deltaTime, ForceMode2D.Impulse);
        }

        // Apply drag to velocity
        rb.velocity *= drag;
    }
}
