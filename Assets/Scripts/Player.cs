using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    [SerializeField] private float moveSpeed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float MoveX = 0f;
        float MoveY = 0f;
        
        if (Keyboard.current.upArrowKey.isPressed)
        {
            MoveY = 1f;
        }
        if (Keyboard.current.downArrowKey.isPressed)
        {
            MoveY = -1f;
        }
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            MoveX = -1f;
        }
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            MoveX = 1f;
        }

        moveDirection = new Vector2(MoveX, MoveY).normalized;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * moveSpeed;
    }
}
