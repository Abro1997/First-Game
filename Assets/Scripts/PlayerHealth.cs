using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Stats")]

    [SerializeField] private int MaxHealth = 20;
    [SerializeField] private float iFrames = 1.5f;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (Time.time < iFrames)
            return;

        iFrames = Time.time + iFrames;
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

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
