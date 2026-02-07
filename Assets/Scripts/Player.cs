using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    [SerializeField] private float moveSpeed = 5f;

    public event EventHandler<OnPlayerMoneyPickup> OnMoneyPickup;
    public class OnPlayerMoneyPickup : EventArgs
    {
        public int moneyValue;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        PlayerStats stats = PlayerStats.Instance;
        moveSpeed = stats.GetMoveSpeed();

        playerHealth.OnPlayerDied += OnPlayerDied;
    }

    private void Update()
    {
        float x = 0f;
        float y = 0f;

        if (GameInput.Instance.IsUpActionPressed()) y = 1f;
        if (GameInput.Instance.IsDownActionPressed()) y = -1f;
        if (GameInput.Instance.IsLeftActionPressed()) x = -1f;
        if (GameInput.Instance.IsRightActionPressed()) x = 1f;

        moveDirection = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }

    public void TakeDamage(int damage)
    {
        playerHealth.TakeDamage(damage);
    }

    private void OnPlayerDied(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnPlayerDied -= OnPlayerDied;
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
}
