using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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
        Shop
    }
    private GameState currentState;

    private void Awake()
    {
        Instance = this;
        waveNumber = 1000;
        currentState = GameState.Playing;
        ExitShop();
    }
    private void Start()
    {
        player.OnMoneyPickup += Player_OnMoneyPickup;
        enemySpawner.OnWaveEnd += EnemySpawner_OnWaveEnd;
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
        Time.timeScale = 1f;
        ExitShop();
        if (currentShopInstance != null)
        {
            Destroy(currentShopInstance);
        }
        player.gameObject.SetActive(true);

        waveNumber++;
        currentState = GameState.Playing;
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
        {
            return;
        }
        shopCanvas.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }
    public bool TrySpendMoney(int amount)
    {
        if (money < amount)
            return false;

        money -= amount;
        return true;
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
}
