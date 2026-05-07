using Panda;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class BTEnemyController : MonoBehaviour
{
    public Transform player;
    public Collider collider;
    public float detectionRange = 10f;
    public float fleeRange = 3f;
    public float health = 100f;
    public Vector3 goToPosition;
    private MovementController movementController;
    [SerializeField] private float arrivedDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementController = GetComponent<MovementController>();
        if (collider != null) goToPosition = GetRandomPointInCollider();
    }
    [Task]
    bool IsLowHealth()
    {
        return health < 30;
    }
    [Task]
    bool IsPlayerInRange()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }

    #region Movement Actions
    [Task]
    public void Chase()
    {
        movementController.MoveTowards(player.position);
        Task.current.Succeed();
    }
    [Task]
    public void Patrol()
    {
        movementController.MoveTowards(goToPosition);

        if (Vector3.Distance(transform.position, goToPosition) <= arrivedDistance)
        {
            goToPosition = GetRandomPointInCollider(); 
            Task.current.Succeed();
        }
    }
    [Task]
    public void Flee()
    {
        movementController.MoveAway(player.position);
        Task.current.Succeed();
    }
    #endregion
    #region Visual Range
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, fleeRange);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 3f);
    }
    #endregion
    private Vector3 GetRandomPointInCollider()
    {
        Vector3 position = new Vector3(Random.Range(collider.bounds.min.x, collider.bounds.max.x), transform.position.y, Random.Range(collider.bounds.min.z, collider.bounds.max.z));
        return position;
    }
}
