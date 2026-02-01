using UnityEngine;

public class MoneyCountUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;

    private void Update()
    {
        int currentMoney = GameManager.Instance.GetMoney();
        moneyText.text = "Money: " + currentMoney.ToString() + "Dh";
    }
}
