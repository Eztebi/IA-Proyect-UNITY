using UnityEngine;

public class SimpleEnemyController : MonoBehaviour
{
    public Transform player;
    public float detectionRange;
    public float fleeRange = 3f;
    public float health = 100f;

    private MovementController controller;

    private void Start()
    {
        controller = this.GetComponent<MovementController>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, this.transform.position);

        if (health < 30 && distance < detectionRange) Flee();
        else if (distance < detectionRange) Chase();
        else Patrol();
    }

    public void Chase()
    {
        controller.MoveTowards(player.position);
    }
    public void Patrol()
    {
        transform.Rotate(0,50 * Time.deltaTime, 0);
        health = health > 100 ? health++ : health;

    }
    public void Flee()
    {
        controller.MoveAway(player.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, fleeRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
    }
}

