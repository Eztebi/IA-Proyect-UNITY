using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerMovement playerRef;

    [SerializeField] private float maxRadius = 10f;

    private void Awake()
    {
        playerRef = GetComponent<PlayerMovement>();
    }

    private void OnDrawGizmos()
    {
        if (playerRef == null) return;

        float noise = playerRef.GetNoise();
        float radius = noise * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
