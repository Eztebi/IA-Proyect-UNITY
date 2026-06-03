using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class CoordinatedEnemyController : MonoBehaviour
{
    public enum EnemyBehaviour
    {
        atacker, surrounder, follower, waiter
    }

    [Header("Referencias")]
    public Transform player;
    public float speed = 3f;
    public float separationDistance = 2f;
    public float separationForce = 3f;
    public float stopDistance = 3f;

    public int enemyIndex;
    public int totalEnemies = 8;
    public float surroundRadius = 5f;

    public EnemyBehaviour behaviour = EnemyBehaviour.follower;
    public float preferedDist = 6f;

    private void Update()
    {
        switch (behaviour)
        {
            case EnemyBehaviour.follower:
                FollowBehaviour();
                break;

            case EnemyBehaviour.atacker:
                AttackBehaviour();
                break;

            case EnemyBehaviour.surrounder:
                SurroundBehaviour();
                break;

            case EnemyBehaviour.waiter:
                WaitBehaviour();
                break;

        }
    }
    void FollowBehaviour()
    {
        Vector3 targetPos = GetSurroundedPosition();
        if (targetPos.magnitude > stopDistance)
        {
            Vector3 direction = (targetPos - transform.position).normalized;
            Vector3 separation = Separate() * separationForce;
            Vector3 finalDir = direction + separation;
            finalDir.y = 0;
            transform.position += finalDir * speed * Time.deltaTime;
        }        
    }

    void AttackBehaviour()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 separation = Separate() * separationForce;
        Vector3 finalDir = dir + separation;
        finalDir.y = 0;
        Vector3.ClampMagnitude(finalDir, 1);

        transform.position += finalDir * speed * Time.deltaTime;

    }

    void SurroundBehaviour()
    {
        Vector3 targetPos = GetSurroundedPosition();
        Vector3 direction = (targetPos - transform.position).normalized;
        if (direction.magnitude > stopDistance)
        {
            Vector3 separation = Separate() * separationForce;
            Vector3 finalDir = direction + separation;
            finalDir.y = 0;
            Vector3.ClampMagnitude(finalDir, 1);
            transform.position += finalDir * speed * Time.deltaTime;
        }
    }
    void WaitBehaviour()
    {
        Vector3 toPlayer = player.position - transform.position;

        float distance = toPlayer.magnitude;
        Vector3 dir = Vector3.zero;
        if (distance < preferedDist - 1f) {
            dir = -toPlayer.normalized;
        }
        else if (distance > preferedDist + 1f) {
            dir = toPlayer.normalized;
        }
        else return;

        Vector3 separation = Separate();
        Vector3 finalDir = dir + separation;
        finalDir.y = 0;
        Vector3.ClampMagnitude(finalDir, 1);

        transform.position += finalDir * speed * Time.deltaTime;
    }

    Vector3 Separate()
    {
        Vector3 force = Vector3.zero;

        Collider[] neightbors = Physics.OverlapSphere(transform.position, separationDistance);

        foreach (Collider n in neightbors)
        {
            if (n.gameObject != gameObject && n.CompareTag("Enemy"))
            {
                Vector3 away = transform.position - n.transform.position;
                float strength = Mathf.Clamp01((separationDistance - away.magnitude) / separationDistance);
                force += away.normalized * strength;
            }
        }

        return force;
    }

    Vector3 GetSurroundedPosition()
    {
        float angle = (360 /totalEnemies) * enemyIndex;
        float radians = Mathf.Deg2Rad * angle;
        Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians)) * surroundRadius;
        return player.position + offset;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.dodgerBlue;
        Gizmos.DrawWireSphere(transform.position, separationDistance);
        Gizmos.color = Color.darkSalmon;
        Gizmos.DrawLine(transform.position, player.position);
        
        if(behaviour == EnemyBehaviour.surrounder || behaviour == EnemyBehaviour.follower)
        {
            Gizmos.color = Color.cornsilk;
            Gizmos.DrawWireSphere(player.position, surroundRadius);
            Vector3 surroundPos = GetSurroundedPosition();
            Gizmos.color = Color.crimson;
            Gizmos.DrawSphere(surroundPos, 0.2f);
            Gizmos.color = Color.teal;
            Gizmos.DrawLine(surroundPos, transform.position);
        }

        if(behaviour == EnemyBehaviour.waiter)
        {
            Gizmos.color = Color.limeGreen;
            Gizmos.DrawWireSphere(player.position, preferedDist);
        }
    }

}
