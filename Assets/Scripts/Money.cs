using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] private int moneyValue = 20;
    public void DestroyMoney()
    {
        Destroy(gameObject);
    }
    public int GetMoneyValue()
    {
        return moneyValue;
    }
}
