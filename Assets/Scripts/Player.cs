using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Transform playerTransform;
    [SerializeField] private float moveSpeed = 5f;
    public event EventHandler<OnPlayerHealthChanged> OnHealthChanged;
    public class OnPlayerHealthChanged : EventArgs
    {
        public int currentHealth;
    }

    [Header("Health Stats")]

    [SerializeField] private int MaxHealth = 20;
    [SerializeField] private float iFrames = 1.5f;

    private int currentHealth;
    private float lastDamageTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerTransform = GetComponent<Transform>();
        currentHealth = MaxHealth;
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
}
