using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 180f;

    [SerializeField]
    private float thrustPower = 10f;

    [SerializeField]
    private float drag = 0.99f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.drag = 0f; 
        rb.angularDrag = 0f; 
    }

    void Update()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward, -rotationInput * rotationSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.up * thrustPower * Time.deltaTime, ForceMode2D.Impulse);
        }else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * thrustPower/3 * Time.deltaTime, ForceMode2D.Impulse);
        }

        rb.velocity *= drag;
    }
}
