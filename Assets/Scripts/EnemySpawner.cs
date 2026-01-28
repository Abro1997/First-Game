using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private GameObject enemyPrefab;


    private float spawnInterval = 3f;
    private int maxEnemies = 20;
    private float spawnOffset = 1.5f;

    private Vector2 spawnPos;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && CountEnemies() < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        spawnPos = Vector2.zero;
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Top
                spawnPos = new Vector2(Random.Range(-camWidth, camWidth), camHeight + spawnOffset);
                break;
            case 1: // Bottom
                spawnPos = new Vector2(Random.Range(-camWidth, camWidth), -camHeight - spawnOffset);
                break;

            case 2: // Left
                spawnPos = new Vector2(-camWidth - spawnOffset, Random.Range(-camHeight, camHeight));
                break;
            case 3: // Right
                spawnPos = new Vector2(camWidth + spawnOffset, Random.Range(-camHeight, camHeight));
                break;
        }
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
    private int CountEnemies()
    {
        return Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
    }
}
