using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Player player;
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
        player.OnHealthChanged += Player_OnHealthChanged;
    }

    private void OnDisable()
    {
        player.OnHealthChanged -= Player_OnHealthChanged;
    }

    private void Start()
    {
        healthUI.UpdateHearts(player.GetCurrentHealth());
    }

    private void Player_OnHealthChanged(object sender, Player.OnPlayerHealthChanged e)
    {
        healthUI.UpdateHearts(e.currentHealth);

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
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

        // Movement-based facing fallback
        float xVelocity = playerRigidbody.linearVelocity.x;

        if (xVelocity > 0.01f)
            spriteRenderer.flipX = false;
        else if (xVelocity < -0.01f)
            spriteRenderer.flipX = true;
    }
    private IEnumerator Blink()
    {
        float elapsed = 0f;
        float iFrames = player.GetIFramesDuration();

        while
        (elapsed < iFrames)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval;
        }
        spriteRenderer.enabled = true;
    }


}
