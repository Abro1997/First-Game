using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{   
    
    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;
    private float health;
    private Transform playerTransform;
    public static event EventHandler OnEnemySpawn;

    private void Awake()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        OnEnemySpawn?.Invoke(this, EventArgs.Empty);   
    }
    private void FixedUpdate()
    {
        if (playerTransform != null)
        MoveTowardsPlayer();
    }
    private void Update()
    {
       Player player = UnityEngine.Object.FindFirstObjectByType<Player>();
        if (player != null)
            playerTransform = player.transform;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Player player = collision.collider.GetComponent<Player>();

    if (player != null)
    {
        player.TakeDamage(1);
    }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Vector2 newPosition = rb.position + direction * Time.fixedDeltaTime * moveSpeed;
        rb.MovePosition(newPosition);
    }
    public void TakeDamage(float damage)
    {
        health -= damage;

        rb.AddForce(-transform.right * knockbackForce, ForceMode2D.Impulse);

        if (health <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
