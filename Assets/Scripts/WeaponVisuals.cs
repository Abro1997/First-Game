using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponVisuals : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody2D playerRigidbody;

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
            spriteRenderer.flipY = false; // facing right
            return;
        }
        else if (mouseWorldPos.x < playerTransform.position.x)
        {
            spriteRenderer.flipY = true;  // facing left
            return;
        }
    }
}
