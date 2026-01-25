using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Stats")]
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float iFramesDuration = 1.5f;

    private int currentHealth;
    private float nextDamageTime;


    private void Awake()
    {
        currentHealth = maxHealth;
        nextDamageTime = 0f;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < nextDamageTime)
            return;

        nextDamageTime = Time.time + iFramesDuration;

        currentHealth -= damage;

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
}
