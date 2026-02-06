using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Weapon : MonoBehaviour
{
    [Header("Stretch")]
    [SerializeField] private float minLength = 0.3f;
    [SerializeField] private float maxLength = 0.8f;

    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    private float fireRate;


    private Vector3 firePointBaseLocalPos;

    private float lastFireTime;
    private Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
        firePointBaseLocalPos = firePoint.localPosition;
    }
    private void Start()
    {
        ApplyStats();
        PlayerStats.Instance.OnStatsChanged += OnStatsChanged;
    }

    private void Update()
    {
        AimAndStretch();
        FixFirePointOrientation();

        if (Mouse.current.leftButton.isPressed)
            TryShoot();
    }

    private void AimAndStretch()
    {
        Vector3 mouseWorldPos = GetMouseWorldPosition();

        // Direction & rotation
        Vector2 direction = mouseWorldPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Stretch amount
        float distance = direction.magnitude;
        float clampedDistance = Mathf.Clamp(distance, minLength, maxLength);

        // Scale only on X
        Vector3 scale = initialScale;
        scale.x = clampedDistance;
        transform.localScale = scale;
    }

    private void TryShoot()
    {
        if (Time.time < lastFireTime + fireRate)
            return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        lastFireTime = Time.time;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
        mouseScreenPos.z = Camera.main.nearClipPlane;
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f;
        return mouseWorldPos;
    }
    private void FixFirePointOrientation()
    {
        float z = transform.eulerAngles.z;

        bool aimingLeft = z > 90f && z < 270f;

        Vector3 localPos = firePointBaseLocalPos;
        localPos.y = aimingLeft ? -Mathf.Abs(localPos.y) : Mathf.Abs(localPos.y);
        firePoint.localPosition = localPos;
    }
    private void OnDestroy()
    {
        PlayerStats.Instance.OnStatsChanged -= OnStatsChanged;
    }

    private void OnStatsChanged(object sender, EventArgs e)
    {
        ApplyStats();
    }

    private void ApplyStats()
    {
        fireRate = PlayerStats.Instance.GetFireRate();
    }

}
