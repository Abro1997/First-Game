using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event EventHandler OnPlayerDied;

    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;
    public class OnHealthChangedEventArgs : EventArgs
    {
        public int currentHealth;
        public int maxHealth;
    }

    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float iFramesDuration = 0.25f;

    private int currentHealth;
    private float nextDamageTime;

    private void Awake()
    {
        currentHealth = maxHealth;

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
        {
            currentHealth = currentHealth,
            maxHealth = maxHealth
        });
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < nextDamageTime)
            return;

        nextDamageTime = Time.time + iFramesDuration;

        currentHealth = Mathf.Max(currentHealth - damage, 0);

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
        {
            currentHealth = currentHealth,
            maxHealth = maxHealth
        });

        if (currentHealth <= 0)
            OnPlayerDied?.Invoke(this, EventArgs.Empty);
    }

    public int GetCurrentHealth() => currentHealth;
    public float GetIFramesDuration() => iFramesDuration;
}
