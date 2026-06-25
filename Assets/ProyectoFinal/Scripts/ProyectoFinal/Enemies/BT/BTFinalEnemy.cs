using Panda;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class BTFinalEnemy : MonoBehaviour
{
    public Transform player;
    private PlayerMovement playerRef;

    [Header("Vision")]
    public Transform eyePoint;
    public float viewDistance = 10f;
    public float viewAngle = 60f;

    [Header("Movement")]
    public float arrivedDistance = 1.5f;
    private EnemyFinalController movementController;

    [Header("HidingSpots")]
    public List<Transform> hidingSpots;

    [SerializeField]private Vector3 lastKnownPosition;
    private Vector3 patrolPoint;
    private bool hasLastKnownPosition = false;

    public LineRenderer[] visionLines;
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        movementController = GetComponent<EnemyFinalController>();
        playerRef = player.GetComponent<PlayerMovement>();

        patrolPoint = GetRandomPoint(20f);
        lastKnownPosition = Vector3.zero;
        hasLastKnownPosition = false;
        visionLines = new LineRenderer[10];

        for (int i = 0; i < visionLines.Length; i++)
        {
            GameObject line = new GameObject("VisionLine");
            line.transform.SetParent(this.transform);
            visionLines[i] = line.AddComponent<LineRenderer>();
            visionLines[i].startWidth = 0.02f;
            visionLines[i].endWidth = 0.02f;
        }
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            float halfAngle = viewAngle * 0.5f;
            float step = viewAngle / 9f;

            float angleOffset = -halfAngle + step * i;
            Vector3 dir = Quaternion.Euler(0, angleOffset, 0) * transform.forward;

            Vector3 end = eyePoint.position + dir * viewDistance;

            visionLines[i].SetPosition(0, eyePoint.position);
            visionLines[i].SetPosition(1, end);
        }
    }
    private bool IsPlayerOnSight()
    {
        if (player == null || eyePoint == null || playerRef.IsHidding()) return false;

        Vector3 origin = eyePoint.position;

        float halfAngle = viewAngle * 0.5f;
        float step = viewAngle / 9f;

        for (int i = 0; i < 10; i++)
        {
            float angleOffset = -halfAngle + step * i;
            Vector3 dir = Quaternion.Euler(0, angleOffset, 0) * transform.forward;
            Debug.DrawRay(eyePoint.position, dir * viewDistance, Color.red);
            if (Physics.Raycast(origin, dir, out RaycastHit hit, viewDistance))
            {
                if (hit.transform.root == player)
                {
                    return true;
                }
            }
        }

        return false;
    }
    [Task]
    bool CanSeePlayer()
    {
        return IsPlayerOnSight();
    }
    [Task]
    bool CanHearPlayer()
    {
        if (playerRef == null || player == null) return false;

        float noise = playerRef.GetNoise();
        float dist = Vector3.Distance(transform.position, player.position);

        return noise * playerRef.GetNoiseRaidus() > dist;
    }
    [Task]
    bool ReachedDestination()
    {
        return Vector3.Distance(transform.position, lastKnownPosition) < arrivedDistance;
    }

    [Task]
    public void Chase()
    {
        text.text = "!";
        if (playerRef.IsHidding())
        {
            hasLastKnownPosition = false;
            Task.current.Fail();
        }
        searchAroundTimes = 0;
        lastKnownPosition = player.position;
        movementController.MoveTowards(player.position);
        hasLastKnownPosition = true;
        Task.current.Succeed();
    }

    [Task]
    public void InvestigateSound()
    {
        text.text = "?";
        lastKnownPosition = player.position;
        movementController.MoveTowards(player.position);
        hasLastKnownPosition = true;
        Task.current.Succeed();
    }
    [Task]
    bool HasLastKnownPosition()
    {
        if (!hasLastKnownPosition)
        {
            Task.current.Fail();
            return false;
        }
        else
        {
            Task.current.Succeed();
            return true;
        }
    }
    [Task]
    public void SearchLastPosition()
    {
        text.text = "?";

        movementController.MoveTowards(lastKnownPosition);

        if (Vector3.Distance(transform.position, lastKnownPosition) < arrivedDistance)
        {
        if (IsPlayerOnSight())
        {
            Task.current.Fail();
            return;
        }
            lastKnownPosition = new Vector3(player.transform.position.x + Random.Range(-5f, 5f),player.transform.position.y,player.transform.position.z + Random.Range(-5f, 5f));
            searchAroundTimes++;

            if (searchAroundTimes >= 3)
            {
                Transform hide = GetClosestHidingSpot();

                if (hide != null)
                {
                    lastKnownPosition = hide.position;
                }

                hasLastKnownPosition = false;
                searchAroundTimes = 0;
                Task.current.Succeed();
            }
        }
    }
    Transform GetClosestHidingSpot()
    {
        Transform closest = null;
        float bestDist = 100;
        foreach (var hide in hidingSpots)
        {
            float dist = Vector3.Distance(transform.position, hide.position);
            if (dist < bestDist)
            {
                bestDist = dist;
                closest = hide;
            }
        }
        return closest;
    }
    [SerializeField]private int searchAroundTimes = 0;

    [Task]
    public void Patrol()
    {
        text.text = ".";

        searchAroundTimes = 0;
        hasLastKnownPosition = false;
        if (IsPlayerOnSight())
        {
            Task.current.Fail();
            return;
        }
        if (CanHearPlayer())
        {
            Task.current.Fail();
            return;
        }
        if (!CanReach(patrolPoint))
        {
            Task.current.Fail();
            patrolPoint = GetRandomPoint(20f);
            return;
        }

        movementController.MoveTowards(patrolPoint);

        if (Vector3.Distance(transform.position, patrolPoint) < arrivedDistance + 1f)
        {
            patrolPoint = GetRandomPoint(20f);
            Task.current.Succeed();
        }
    }
    Vector3 GetRandomPoint(float range)
    {
        return new Vector3(transform.position.x + Random.Range(-range, range),transform.position.y,transform.position.z + Random.Range(-range, range));
    }
    bool CanReach(Vector3 target)
    {
        NavMeshPath path = movementController.agent.path;
        movementController.agent.CalculatePath(target, path);

        return path.status == NavMeshPathStatus.PathComplete;
    }
    private void OnDrawGizmos()
    {
        // last player pos
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lastKnownPosition, 0.3f);

        // random patrol point
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(patrolPoint, 0.3f);
    }
}