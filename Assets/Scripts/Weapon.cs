using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.5f;

    private float lastFireTime;
    private void Aim()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        Vector2 direction = mouseWorldPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void TryShoot()
    {
        if (Time.time < lastFireTime + fireRate) return;
        
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        lastFireTime = Time.time;
    }
    private void Update()
    {
        Aim();

        if (Mouse.current.leftButton.isPressed)
        {
            TryShoot();
        }
    }
}
