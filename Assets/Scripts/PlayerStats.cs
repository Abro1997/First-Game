using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    
    public event EventHandler OnStatsChanged;

    [Header("Player Stats")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private int maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private float fireRate;


    private void Awake()
    {
        Instance = this;

        moveSpeed = 5f;
        maxHealth = 20;
        damage = 10;
        fireRate = 0.5f;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    public int GetDamage()
    {
        return damage;
    }
    public float GetFireRate()
    {
        return fireRate;
    }
    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;
        OnStatsChanged?.Invoke(this, EventArgs.Empty);
    }
    public void IncreaseDamage(int amount)
    {
        damage += amount;
        OnStatsChanged?.Invoke(this, EventArgs.Empty);
    }
    public void DecreaseFireRate(float amount)
    {
        fireRate -= amount;
        OnStatsChanged?.Invoke(this, EventArgs.Empty);
    }
}
