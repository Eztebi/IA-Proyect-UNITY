using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Transform player;
    public PlayerMovement playerRef;
    public EnemyFinalController movementController;
    public Vector3 point;
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        movementController = GetComponent<EnemyFinalController>();
        point = GetRandomPoint(5f);
        text = GetComponentInChildren<TextMeshProUGUI>();

    }

    void Update()
    {
        float noise = playerRef.GetNoise(); 
        float distance = Vector3.Distance(transform.position, player.position);

        float lowNoise = LowNoise(noise);
        float midNoise = MidNoise(noise);
        float highNoise = HighNoise(noise);
        float nearDist = NearDistance(distance);
        float midDist = MidDistance(distance);
        float farDist = FarDistance(distance);

        float rulePatrol = AND(lowNoise, farDist);
        float ruleInvestigate = AND(midNoise, midDist);
        float ruleChase = AND(highNoise, nearDist);
        
        float patrol = rulePatrol * .5f;
        float investigate = ruleInvestigate * .8f;
        float chase = ruleChase * 1.0f;

        Debug.Log($"Patrol:{patrol:F2} Investigate:{investigate:F2} Chase:{chase:F2}");

        if (chase > investigate && chase > patrol)
        {
            Chase();
        }
        else if (investigate > patrol)
        {
            Investigate();
        }
        else
        {
            Patrol();
        }
    }
    public void Patrol()
    {
        if (!CanReach(point))
        {
            point = GetRandomPoint(20f);
            return;
        }
        text.text = ".";

        movementController.MoveTowards(point);

        if (Vector3.Distance(transform.position, point) < 1.5f + 1f)
        {
            point = GetRandomPoint(20f);
        }
    }

    void Investigate()
    {
        if (!CanReach(point))
        {
            Vector3 point = new Vector3(transform.position.x + Random.Range(-5, 5),transform.position.y,transform.position.z + Random.Range(-5, 5));
        }
        text.text = "?";

        if (Vector3.Distance(transform.position, point) < 1.5f + 1f)
        {
            point = GetRandomPoint(10f);
        }
    }

    void Chase()
    {
        text.text = "!";

        movementController.MoveTowards(player.position);
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
    float LowNoise(float noise)
    {
        if (noise <= 0.2f) return 1;
        if (noise >= 0.4f) return 0;
        return (0.4f - noise) / 0.2f;
    }

    float MidNoise(float noise)
    {
        if (noise <= 0.2f || noise >= 0.8f) return 0;
        if (noise == 0.5f) return 1;

        if (noise < 0.5f) return (noise - 0.2f) / 0.3f;

        return (0.8f - noise) / 0.3f;
    }

    float HighNoise(float noise)
    {
        if (noise <= 0.5f) return 0;
        if (noise >= 1f) return 1;
        return (noise - 0.5f) / 0.5f;
    }
    float NearDistance(float distance)
    {
        if (distance <= 5f) return 1;
        if (distance >= 12f) return 0;
        return (12f - distance) / 7f;
    }

    float MidDistance(float distance)
    {
        if (distance <= 5f || distance >= 20f) return 0;
        if (distance == 10f) return 1;

        if (distance < 10f) return (distance - 5f) / 5f;

        return (20f - distance) / 10f;
    }

    float FarDistance(float distance)
    {
        if (distance <= 10f) return 0;
        if (distance >= 25f) return 1;
        return (distance - 10f) / 15f;
    }
    float AND(float a, float b) => Mathf.Min(a, b);
    float OR(float a, float b) => Mathf.Max(a, b);
}
