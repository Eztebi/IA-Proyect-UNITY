using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MovementController : MonoBehaviour
{
    public float speed = 3;

    public void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - this.transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
    public void MoveAway(Vector3 target)
    {
        Vector3 direction = (this.transform.position - target).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }
}
