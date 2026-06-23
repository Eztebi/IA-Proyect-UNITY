using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;

    [SerializeField] private float noiseIncrese = 0.6f;
    [SerializeField] private float noiseDecrese = 1f;

    private Rigidbody rb;
    private Vector3 direction;

    private bool isRunning;
    private float noise; 

    public float GetNoise() => noise;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        direction = new Vector3(moveX, 0f, moveZ).normalized;
        isRunning = Input.GetKey(KeyCode.LeftShift);
     
        if (direction.magnitude > 0.1f)
        {
            float speed = isRunning ? 1f : 0.5f;
            noise += Time.deltaTime * noiseIncrese * speed;
            if (!isRunning && noise >= .5) noise = .5f;
        }
        else
        {
            noise = 0;
        }
        noise = Mathf.Clamp01(noise);
    }

    void FixedUpdate()
    {
        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector3(direction.x * speed,rb.linearVelocity.y,direction.z * speed);
    }
}