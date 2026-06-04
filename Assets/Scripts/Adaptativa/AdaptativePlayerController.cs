using UnityEngine;

public class AdaptativePlayerController : MonoBehaviour
{
    public float attackRange = 3f;
    public float speed = 5f;
    public float damage = 10f;
    public LayerMask enemies;
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0, v);
        transform.Translate(movement * speed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
    void Attack()
    {
        Collider[] aux = Physics.OverlapSphere(transform.position, attackRange, enemies);

        if (aux.Length != 0)
        {
            Destroy(aux[0].gameObject);
            DifficultyManager.Instance.RegisterKill();
            DifficultyManager.Instance.RegisterAccuracy(1);
        }
        else
        {
            DifficultyManager.Instance.RegisterAccuracy(0);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            DifficultyManager.Instance.RegisterDamageTaken(10);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
