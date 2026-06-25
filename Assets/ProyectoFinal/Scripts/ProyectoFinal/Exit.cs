using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            GameManager.Instance.Win();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.Win();
        }
    }
}
