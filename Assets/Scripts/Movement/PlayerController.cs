using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 10f;
    public FuzzyController sEnemy;
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
        sEnemy.health -= damage;
    }
}
