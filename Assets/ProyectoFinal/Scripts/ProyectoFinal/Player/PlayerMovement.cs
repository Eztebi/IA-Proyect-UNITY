using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;

    [SerializeField] private float noiseIncrese = 0.6f;
    [SerializeField] private float noiseDecrese = 1f;

    private Rigidbody rb;
    private Collider collider;
    private MeshRenderer renderer;
    private Vector3 direction;
    private bool isHiding;
    public bool canHide;

    private bool isRunning;
    private float noise;
    private bool isDeath;
    [SerializeField]private float noiseRadius = 5f; 

    public float GetNoise() => noise;
    public float GetNoiseRaidus() => noiseRadius;
    public bool IsHidding() => isHiding;
    public bool IsDeaath()=> isDeath;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<MeshRenderer>();
        isHiding = false;
    }

    void Update()
    {
        if (isDeath) return;

        if (canHide && Input.GetKeyDown(KeyCode.Space))
        {
            if (isHiding)
                ExitHide();
            else
                Hide();
        }
        if (isHiding)
        {
            noise = 0;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        direction = new Vector3(moveX, 0f, moveZ).normalized;
        isRunning = Input.GetKey(KeyCode.LeftShift);
     
        if (direction.magnitude > 0.1f)
        {
            float speed = isRunning ? 1f : 0.5f;
            noise = speed;
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
        if (isDeath || isHiding)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        float speed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector3(direction.x * speed,rb.linearVelocity.y,direction.z * speed);
    }
    public void Die()
    {
        isDeath = true;
        GameManager.Instance.Lose();
    }
    public void Hide()
    {
        isHiding = true;
        collider.enabled = false;
        renderer.enabled = false;
    }
    public void ExitHide()
    {
        isHiding = false;
        collider.enabled = true;
        renderer.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            FloorCell floor = other.GetComponent<FloorCell>();

            if (floor == null) return;

            switch (floor.type)
            {
                case FloorType.low:
                    noiseRadius = 5f;
                    break;
                case FloorType.medium:
                    noiseRadius = 10f;
                    break;
                case FloorType.high:
                    noiseRadius = 15f;
                    break;
            }
        }
    }
    public void NotPlaying()
    {
        isHiding = true;
        collider.enabled = false;
        renderer.enabled = false;
    }
    public void StartPlaying()
    {
        isHiding = false;
        collider.enabled = true;
        renderer.enabled = true;
    }
}