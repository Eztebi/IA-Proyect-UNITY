using UnityEngine;
using Panda;
using UnityEngine.AI;
public class EnemyControllerTactic : MonoBehaviour
{
    public Transform player, goal;
    public float health;
    public NavMeshAgent agente;
    public Tactics tacticas;
    public float lowHealth = 30;

    [Task] 
    public bool IsLowHealth (){
        return health < lowHealth;
    }
    [Task]
    public void Idle()
    {
        Task.current.Succeed();
    }
    [Task]
    public void Attack()
    {
        Debug.Log("Waaos");
        Task.current.Succeed(); 
    }
    [Task]
    public void MoveToAttackPoint()
    {
        if (goal == null)
        {
            goal = tacticas.GetBestPoint(IAEstado.Attack);
        }
        if(goal == null)
        {
            Task.current.Fail();
            return;
        }

        agente.SetDestination(goal.position);

        if(!agente.pathPending && agente.remainingDistance < 1.5f)
        {
            goal = null;
            Task.current.Succeed();
        }
    }
    [Task]
    public void MoveToSafePoint()
    {
        if (goal == null)
        {
            goal = tacticas.GetBestPoint(IAEstado.Flee);
        }
        if(goal == null)
        {
            Task.current.Fail();
            return;
        }

        agente.SetDestination(goal.position);

        if(!agente.pathPending && agente.remainingDistance < 1.5f)
        {
            goal = null;
            Task.current.Succeed();
        }
    }
    [Task]
    public void MoveToRangeAttackPoint()
    {
        if (goal == null)
        {
            goal = tacticas.GetBestPoint(IAEstado.RangedAttack);
        }
        if (goal == null)
        {
            Task.current.Fail();
            return;
        }

        agente.SetDestination(goal.position);

        if (!agente.pathPending && agente.remainingDistance < 1.5f)
        {
            goal = null;
            Task.current.Succeed();
        }
    }
    [Task]
    public bool IsPlayerFar()
    {
        return Vector3.Distance(transform.position, player.position) > 20f;
    }
    [Task]
    public bool IsPlayerClose()
    {
        return Vector3.Distance(transform.position, player.position) <= 7f;
    }
}
