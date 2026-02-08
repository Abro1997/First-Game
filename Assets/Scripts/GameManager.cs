using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private GameObject shop;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private Player player;

    public static GameManager Instance { get; private set; }

    private static int waveNumber;
    private int money;
    private GameObject currentShopInstance;

    public enum GameState
    {
        Playing,
        Shop,
        GameOver
    }

    private GameState currentState;

    private void Awake()
    {
        // Singleton safety (optional but recommended)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        waveNumber = 1;
        currentState = GameState.Playing;

        ExitShop();
        Time.timeScale = 1f;
    }

    private void Start()
    {
        player.OnMoneyPickup += Player_OnMoneyPickup;
        enemySpawner.OnWaveEnd += EnemySpawner_OnWaveEnd;

        // Make sure gameplay music is on when the game starts
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayMusic(SoundManager.MusicState.Gameplay);
    }

    private void ShopUI_OnBuyDamage(object sender, System.EventArgs e)
    {
        PlayerStats.Instance.IncreaseDamage(1);
    }

    private void ShopUI_OnBuySpeed(object sender, System.EventArgs e)
    {
        PlayerStats.Instance.IncreaseMoveSpeed(0.5f);
    }

    private void ShopUI_OnBuyFireRate(object sender, System.EventArgs e)
    {
        PlayerStats.Instance.DecreaseFireRate(0.1f);
    }


    private void EnemySpawner_OnWaveEnd(object sender, System.EventArgs e)
    {
        if (currentState != GameState.Playing)
            return;

        EnterShopState();
    }

    private void EnterShopState()
    {
        if (currentState == GameState.Shop)
            return;

        currentState = GameState.Shop;

        currentShopInstance = Instantiate(shop, Vector3.zero, Quaternion.identity);
    }

    public void StartNextWave()
    {
        if (currentState != GameState.Shop)
            return;

        Time.timeScale = 1f;

        ExitShop();

        if (currentShopInstance != null)
            Destroy(currentShopInstance);

        player.gameObject.SetActive(true);

        waveNumber++;
        currentState = GameState.Playing;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayMusic(SoundManager.MusicState.Gameplay);
    }

    private void Player_OnMoneyPickup(object sender, Player.OnPlayerMoneyPickup e)
    {
        AddMoney(e.moneyValue);
    }

    private void AddMoney(int amount)
    {
        money += amount;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public int GetMoney()
    {
        return money;
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public bool TrySpendMoney(int amount)
    {
        if (money < amount)
            return false;

        money -= amount;
        return true;
    }

    private void EnterShop()
    {
        shopCanvas.gameObject.SetActive(true);
    }

    private void ExitShop()
    {
        shopCanvas.gameObject.SetActive(false);
    }

    public void OpenShopUI()
    {
        if (currentState != GameState.Shop)
            return;

        shopCanvas.gameObject.SetActive(true);
        player.gameObject.SetActive(false);

        Time.timeScale = 0f;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayMusic(SoundManager.MusicState.Shop);
    }

    public void RegisterShop(ShopUI shopUI)
    {
        shopUI.OnBuyDamage += ShopUI_OnBuyDamage;
        shopUI.OnBuySpeed += ShopUI_OnBuySpeed;
        shopUI.OnBuyFireRate += ShopUI_OnBuyFireRate;
    }

    public void UnregisterShop(ShopUI shopUI)
    {
        shopUI.OnBuyDamage -= ShopUI_OnBuyDamage;
        shopUI.OnBuySpeed -= ShopUI_OnBuySpeed;
        shopUI.OnBuyFireRate -= ShopUI_OnBuyFireRate;
    }

    public void OnPlayerDied()
    {
        if (currentState == GameState.GameOver)
            return;

        currentState = GameState.GameOver;

        ExitShop();

        if (currentShopInstance != null)
        {
            Destroy(currentShopInstance);
            currentShopInstance = null;
        }

        Time.timeScale = 0f;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayMusic(SoundManager.MusicState.GameOver);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        if (SoundManager.Instance != null)
            SoundManager.Instance.ForcePlayMusic(SoundManager.MusicState.Gameplay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
