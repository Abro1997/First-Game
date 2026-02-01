using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public event System.EventHandler OnWaveEnd;

    [Header("Enemy")]
    [SerializeField] private GameObject[] enemyPrefab;

    private GameObject normalEnemy;
    private GameObject fastEnemy;


    [SerializeField] private float spawnOffset;
    private float spawnInterval;
    private int maxEnemiesOnScreen;
    private int enemiesSpawned;
    private int enemiesPerWave;
    private int fastEnemyTypeChance;
    private int fastEnemyWave;
    private int fastEnemyChanceIncrement;

    private Vector2 spawnPos;
    private float timer;

    private GameManager gameManager;

    private void Awake()
    {
        spawnInterval = 2f;
        maxEnemiesOnScreen = 10;
        spawnOffset = 1f;

        enemiesSpawned = 0;
        enemiesPerWave = 5;
        timer = 0f;

        normalEnemy = enemyPrefab[0];

        // Setup fast enemy parameters
        fastEnemy = enemyPrefab[1];
        fastEnemyTypeChance = 0;
        fastEnemyWave = 3;
        fastEnemyChanceIncrement = 10;
    }

    private  void Start()
    {
        gameManager = GameManager.Instance;
    }
    private void Update()
    {
        switch(gameManager.GetCurrentState())
        {
            case GameManager.GameState.Playing:
                HandleSpawning();
                break;
            case GameManager.GameState.Shop:
                // Do nothing or handle shop logic
                break;
        }

    }

    private void SpawnEnemy()
    {

        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize;
        float camWidth = cam.aspect * camHeight;

        spawnPos = Vector2.zero;
        int side = Random.Range(0, 4);
        int enemyTypeRoll = Random.Range(0, 100);
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
        if (enemyTypeRoll < fastEnemyTypeChance)
        {
            Instantiate(fastEnemy, spawnPos, Quaternion.identity);
            return;
        }
        Instantiate(normalEnemy, spawnPos, Quaternion.identity);

    }
    private int CountEnemies()
    {
        return Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
    }
    private int SpawnChanceByWave(int enemyTypeChance, int enemyTypeWave, int chanceIncrement)
    {
        return Mathf.Min(30, enemyTypeChance + chanceIncrement) * Mathf.Min(Mathf.Abs(GameManager.Instance.GetWaveNumber() - enemyTypeWave), 1);
    }
    private void HandleSpawning()
    {
        
        GameManager.Instance.GetWaveNumber();
        timer += Time.deltaTime;

        if (timer >= spawnInterval && CountEnemies() < maxEnemiesOnScreen && enemiesSpawned < enemiesPerWave)
        {
            SpawnEnemy();
            enemiesSpawned++;
            timer = 0f;
        }
        if (enemiesSpawned >= enemiesPerWave && CountEnemies() == 0)
        {
            enemiesSpawned = 0;
            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.05f);
            enemiesPerWave = enemiesPerWave + 2 * GameManager.Instance.GetWaveNumber();
            fastEnemyTypeChance = SpawnChanceByWave(fastEnemyTypeChance, fastEnemyWave, fastEnemyChanceIncrement);
            OnWaveEnd?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
