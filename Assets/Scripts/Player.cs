using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Transform playerTransform;
    [SerializeField] private float moveSpeed = 5f;
    public event EventHandler<OnPlayerMoneyPickup> OnMoneyPickup;
    public class OnPlayerMoneyPickup : EventArgs
    {
        public int moneyValue;
    }
    public event EventHandler<OnPlayerHealthChanged> OnHealthChanged;
    public class OnPlayerHealthChanged : EventArgs
    {
        public int currentHealth;
    }

    [SerializeField] private float iFrames = 1.5f;

    private int currentHealth;
    private float lastDamageTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
    }
    private void Start()
    {
        PlayerStats stats = PlayerStats.Instance;
        moveSpeed = stats.GetMoveSpeed();
        currentHealth = stats.GetMaxHealth();
        PlayerStats.Instance.OnStatsChanged += OnStatsChanged;
    }

    private void Update()
    {
        float MoveX = 0f;
        float MoveY = 0f;

        if (GameInput.Instance.IsUpActionPressed())
        {
            MoveY = 1f;
        }

        if (GameInput.Instance.IsDownActionPressed())
        {
            MoveY = -1f;
        }
        if (GameInput.Instance.IsLeftActionPressed())
        {
            MoveX = -1f;
        }
        if (GameInput.Instance.IsRightActionPressed())
        {
            MoveX = 1f;
        }


        moveDirection = new Vector2(MoveX, MoveY).normalized;

    }
    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Money>(out Money money))
        {
            OnMoneyPickup?.Invoke(this, new OnPlayerMoneyPickup
            {
                moneyValue = money.GetMoneyValue()
            });
            money.DestroyMoney();
        }
    }


    public void TakeDamage(int damage)
    {
        if (Time.time < lastDamageTime)
            return;

        lastDamageTime = Time.time + iFrames;
        currentHealth -= damage;

        OnHealthChanged?.Invoke(this, new OnPlayerHealthChanged { currentHealth = currentHealth });

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        Destroy(gameObject);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetIFramesDuration()
    {
        return iFrames;
    }
    private void OnDestroy()
    {
        PlayerStats.Instance.OnStatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged(object sender, EventArgs e)
    {
        ApplyStats();
    }

    private void ApplyStats()
    {
        PlayerStats stats = PlayerStats.Instance;
        moveSpeed = stats.GetMoveSpeed();
        currentHealth = Mathf.Min(currentHealth, stats.GetMaxHealth());
    }
}
