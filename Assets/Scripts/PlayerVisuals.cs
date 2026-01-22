using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform playerTransform;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Mouse-based facing
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        if (mouseWorldPos.x > playerTransform.position.x)
        {
            spriteRenderer.flipX = false; // face right
            return;
        }
        else if (mouseWorldPos.x < playerTransform.position.x)
        {
            spriteRenderer.flipX = true;  // face left
            return;
        }

        //movement-based facing
        float xVelocity = playerRigidbody.linearVelocity.x;

        if (xVelocity > 0.01f)
            spriteRenderer.flipX = false;
        else if (xVelocity < -0.01f)
            spriteRenderer.flipX = true;
    }
}
