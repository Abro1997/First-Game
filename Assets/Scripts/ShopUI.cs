using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button buyDamageButton;
    [SerializeField] private Button buySpeedButton;
    [SerializeField] private Button buyFireRateButton;
    [SerializeField] private Button nextWaveButton;

    [Header("Costs")]
    [SerializeField] private int damageCost = 10;
    [SerializeField] private int speedCost = 15;
    [SerializeField] private int fireRateCost = 20;

    private void Awake()
    {
        buyDamageButton.onClick.AddListener(OnBuyDamageClicked);
        buySpeedButton.onClick.AddListener(OnBuySpeedClicked);
        buyFireRateButton.onClick.AddListener(OnBuyFireRateClicked);
        nextWaveButton.onClick.AddListener(OnNextWaveClicked);
    }

    private void OnDestroy()
    {
        buyDamageButton.onClick.RemoveListener(OnBuyDamageClicked);
        buySpeedButton.onClick.RemoveListener(OnBuySpeedClicked);
        buyFireRateButton.onClick.RemoveListener(OnBuyFireRateClicked);
        nextWaveButton.onClick.RemoveListener(OnNextWaveClicked);
    }
        private void OnBuyDamageClicked()
    {
        if (GameManager.Instance.TrySpendMoney(damageCost))
        {
            Debug.Log("Damage upgraded");
            // Apply upgrade later
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    private void OnBuySpeedClicked()
    {
        if (GameManager.Instance.TrySpendMoney(speedCost))
        {
            Debug.Log("Speed upgraded");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    private void OnBuyFireRateClicked()
    {
        if (GameManager.Instance.TrySpendMoney(fireRateCost))
        {
            Debug.Log("Fire rate upgraded");
        }
        else
        {
            Debug.Log("Not enough money");
        }
    }

    private void OnNextWaveClicked()
    {
        GameManager.Instance.StartNextWave();
    }
}

