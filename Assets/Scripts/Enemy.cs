using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Coin Stats")]
    [SerializeField] private GameObject commonCoinDrop;
    [SerializeField] private GameObject rareCoinDrop;
    [SerializeField] private int coinDropCount;
    [SerializeField] private float coinDropRadius = 0.5f;
    [SerializeField] private int chanceForRareCoin = 10; // Percentage chance to drop a rare coin

    [Header("Stats")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float knockbackForce = 2f;
    [SerializeField] private float moveSpeed = 2f;

    private bool isDead = false;
    private Rigidbody2D rb;
    private float health;
    private Transform playerTransform;
    public static event EventHandler OnEnemySpawn;
    public event EventHandler OnEnemyDamaged;


    private void Awake()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();

    }
    private void Start()
    {
        OnEnemySpawn?.Invoke(this, EventArgs.Empty);
    }
    private void OnStatsChanged(object sender, EventArgs e)
    {
        PlayerStats stats = PlayerStats.Instance;
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

        PlayerHealth playerHealth =
            collision.collider.GetComponentInChildren<PlayerHealth>();
            Debug.Log("Found PlayerHealth: " + playerHealth);

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
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
        OnEnemyDamaged?.Invoke(this, EventArgs.Empty);

        rb.AddForce(-transform.right * knockbackForce, ForceMode2D.Impulse);

        if (health <= 0f)
        {
            Die();
        }
    }
    private void Die()
    {
        if (isDead) return;
        isDead = true;
        SpawnCoins();
        Destroy(gameObject);
    }
    private void SpawnCoins()
    {
        for (int i = 0; i < coinDropCount; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, 100);
            GameObject coinToSpawn = commonCoinDrop;
            if (randomIndex < chanceForRareCoin)
            {
                coinToSpawn = rareCoinDrop;
            }
            Vector2 spawnPosition = transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle * coinDropRadius);
            Instantiate(coinToSpawn, spawnPosition, Quaternion.identity);
        }
    }
}
