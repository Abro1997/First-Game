using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 10f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        // Bullet Goes Forward
        rb.linearVelocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.TryGetComponent(out Enemy enemy))
        {
            Vector2 hitDir = (enemy.transform.position - transform.position);

            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);

    }
}
