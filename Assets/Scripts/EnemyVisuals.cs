using System;
using UnityEngine;
using System.Collections;

public class EnemyVisuals : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private float blinkDuration = 0.15f;

    private Coroutine blinkCoroutine;
    private Color originalColor;

    private void Awake()
    {
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        enemy.OnEnemyDamaged += Enemy_OnEnemyDamaged;
    }

    private void OnDisable()
    {
        enemy.OnEnemyDamaged -= Enemy_OnEnemyDamaged;
    }

    private void Enemy_OnEnemyDamaged(object sender, EventArgs e)
    {
        spriteRenderer.color = originalColor;

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(BlinkRed());
    }

    private IEnumerator BlinkRed()
    {
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkInterval);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2f;
        }

        spriteRenderer.color = originalColor;
    }
}
