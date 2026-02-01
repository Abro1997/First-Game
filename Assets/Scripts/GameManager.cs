using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    public static GameManager Instance { get; private set; }
    private static int waveNumber;
    private int money;

    private void Awake()
    {
        Instance = this;
        waveNumber = 1;
    }
    private void Start()
    {
        player.OnMoneyPickup += Player_OnMoneyPickup;
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

    public void IncreaseWaveNumber()
    {
        waveNumber++;
    }
    public int GetMoney()
    {
        return money;
    }

}
