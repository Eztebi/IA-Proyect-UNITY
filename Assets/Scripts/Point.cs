using UnityEngine;

public class Point : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 50f;
    public float coverValue = 0;

    private void Update()
    {
        EvaluateCover();
    }

    void EvaluateCover()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, maxDistance))
        {
            if(hit.transform == player)
            {
                coverValue = 0;
            }
            else
            {
                coverValue = 1;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (player == null) return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 1);
        Gizmos.color = (coverValue > 0.5) ? Color.red: Color.green;
        Gizmos.DrawLine(transform.position, player.position);

    }
}
