using UnityEngine;
using UnityEngine.AI;

public class BayesianEnemyController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;
    public NavMeshAgent agent;

    [Header("Variables Jugador")]
    public float playerHealth = 100;

    [Header("perception")]
    public float visionRange = 10;

    [Header("probabilities")]
    [Range(0f, 1f)] public float priorAttackProbability = 0.5f;

    private float distanceToPlayer;
    private bool playerVisible;

    private float attackProbability;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, visionRange);
        Gizmos.color = playerVisible ? Color.green: Color.red;
        Gizmos.DrawLine(transform.position, player.position);
    }
    private void Update()
    {
        ObservePlayer();
        CalculateBayesianInference();
        DecideAction();
    }
    void ObservePlayer()
    {
        distanceToPlayer = Vector3.Distance(player.position, this.transform.position);

        playerVisible = distanceToPlayer <= visionRange;
            Vector3 direction = (player.position - transform.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, 1000))
            {
                if (hit.transform == player)
                {
                    playerVisible = true;
                }
                else
                {
                    playerVisible = false;
                }
            }
    }
    void CalculateBayesianInference()
    {
        //Prob
        float pDistanceGivenAttack; 
        float pVisibleGivenAttack; 
        float pHealthGivenAttack;

        if (distanceToPlayer < 4f)
        {
            pDistanceGivenAttack = 0.8f;
        }
        else if (distanceToPlayer < 8f)
        {
            pDistanceGivenAttack = 0.5f;
        }
        else {
            pDistanceGivenAttack = 0.3f;

        }

        pVisibleGivenAttack = playerVisible ? 0.7f : 0.2f;

        if (playerHealth < 30)
        {
            pHealthGivenAttack = .3f;
        }
        else
        {
            pHealthGivenAttack = 0.7f;
        }

        ///Como son eventos independientes se multiplican
        attackProbability = priorAttackProbability*pDistanceGivenAttack * pHealthGivenAttack * pVisibleGivenAttack;

        attackProbability = Mathf.Clamp01(attackProbability * 3f);
        Debug.Log("Probabilidad de ataque" + attackProbability);
    }
    void DecideAction()
    {
        if (attackProbability < 0.3f)
        {
            Patrol();
        }
        else if (attackProbability > 0.65)
        {
            Attack();
        }
        else
        {
            ChasePlayer();
        }
    }
    void Attack()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        Debug.Log("Atacar");
    }
    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        Debug.Log("Persiguiendo");
    }
    void Patrol() {
        agent.isStopped = true;
        transform.Rotate(0, 50f * Time.deltaTime, 0);
        Debug.Log("Esperando");
    }
}
