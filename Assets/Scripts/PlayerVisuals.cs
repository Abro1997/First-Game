using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerHealthUI healthUI;
    [SerializeField] private float blinkInterval = 0.1f;

    private Coroutine blinkCoroutine;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged += PlayerHealth_OnHealthChanged;
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged -= PlayerHealth_OnHealthChanged;
    }

    private void Start()
    {
        if (playerHealth != null)
            healthUI.UpdateHearts(playerHealth.GetCurrentHealth());
    }

    private void PlayerHealth_OnHealthChanged(object sender, PlayerHealth.OnHealthChangedEventArgs e)
    {
        healthUI.UpdateHearts(e.currentHealth);

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        blinkCoroutine = StartCoroutine(Blink());
    }


    private void Update()
    {
        // Mouse-based facing
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        if (mouseWorldPos.x > playerTransform.position.x)
            spriteRenderer.flipX = false;
        else if (mouseWorldPos.x < playerTransform.position.x)
            spriteRenderer.flipX = true;

        // Movement-based fallback
        float xVelocity = playerRigidbody.linearVelocity.x;

        if (xVelocity > 0.01f)
            spriteRenderer.flipX = false;
        else if (xVelocity < -0.01f)
            spriteRenderer.flipX = true;
    }

    private IEnumerator Blink()
    {
        float elapsed = 0f;
        float iFrames = playerHealth != null ? playerHealth.GetIFramesDuration() : 0f;

        while (elapsed < iFrames)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        spriteRenderer.enabled = true;
    }
}
