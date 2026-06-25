using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    [SerializeField]private Canvas canvas;

    private void Start()
    {
        canvas.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.enabled = true;
            PlayerMovement playerRef = other.GetComponent<PlayerMovement>();
            playerRef.canHide = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")){
            canvas.enabled = false;
            PlayerMovement playerRef = other.GetComponent<PlayerMovement>();
            playerRef.canHide = false;

        }
    }
}
