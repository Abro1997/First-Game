using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPivot : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Transform playerTransform;

    private Transform weaponPivotTransform;
    private float initialX;

    private void Awake()
    {
        weaponPivotTransform = transform;
        initialX = Mathf.Abs(weaponPivotTransform.localPosition.x);
    }

    private void Update()
    {
        Vector3 pos = weaponPivotTransform.localPosition;

        //Mouse-based side
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        if (mouseWorldPos.x > playerTransform.position.x)
        {
            pos.x = initialX;   // right side
        }
        else if (mouseWorldPos.x < playerTransform.position.x)
        {
            pos.x = -initialX;  // left side
        }
        else
        {
            //movement-based side
            float xVelocity = playerRigidbody.linearVelocity.x;

            if (xVelocity > 0.01f)
                pos.x = initialX;
            else if (xVelocity < -0.01f)
                pos.x = -initialX;
        }

        weaponPivotTransform.localPosition = pos;
    }
}
