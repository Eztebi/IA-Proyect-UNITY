using UnityEngine;
using UnityEngine.AI;

public class EnemyFinalController : MonoBehaviour
{
    public NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTowards(Vector3 target)
    {
        agent.isStopped = false;
        agent.SetDestination(target);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerMovement playerRef = collision.collider.GetComponent<PlayerMovement>();
            if (playerRef != null && !playerRef.IsHidding())
            {
                playerRef.Die();
            }

        }
    }
}