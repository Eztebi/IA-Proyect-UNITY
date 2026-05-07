using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class GoalsEnemyController : MonoBehaviour
{
    public Transform player;
    public float health = 100;
    public float detectionRange = 10f;
    
    [SerializeField] private MovementController movementController;

    private void Start()
    {
        movementController = this.GetComponent<MovementController>();
    }

    private void Update()
    {
        float patrolScrore = EvaluatePatrol();
        float fleeScore = EvaluateFlee();
        float chaseScore = EvaluateChase();

        Debug.Log($"Patrol: {patrolScrore} | Chase: {chaseScore} | Flee: {fleeScore}");

        float maxScore = Mathf.Max(patrolScrore, chaseScore, fleeScore);

        if (maxScore == fleeScore) Flee();
        else if (maxScore == chaseScore) Chase();
        else  Patrol();
    }

    float GetNormalizedDistance()
    {
        float distancce = Vector3.Distance(this.transform.position, player.position);

        return Mathf.Clamp01(distancce / detectionRange);
    }

    //Flee
    float EvaluateFlee()
    {
        //0 == cerca, 1 == lejos
        float distance = GetNormalizedDistance();

        float healthFactor = 1 - (health / 100f);
        
        return healthFactor * (1 - distance);
    }

    //Chase
    float EvaluateChase()
    {
        float distance = GetNormalizedDistance();
        float healthFactor = (health / 100f);

        return(1 - distance) * healthFactor;    
    }

    //Patrol
    float EvaluatePatrol()
    {
        float distance = GetNormalizedDistance();

        return distance;
    }
    public void Chase()
    {
        movementController.MoveTowards(player.position);
    }
    public void Patrol()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        health = health > 100 ? health++ : health;

    }
    public void Flee()
    {
        movementController.MoveAway(player.position);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
    }
}
