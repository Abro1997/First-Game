using UnityEngine;

public class Shop : MonoBehaviour
{
    private bool isPlayerInRange;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            if (GameManager.Instance.GetCurrentState() == GameManager.GameState.Shop)
            {
                isPlayerInRange = true;
                Debug.Log("Press E to enter the shop");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            isPlayerInRange = false;
        }
    }
    private void Update()
    {
        if (!isPlayerInRange)
            return;

        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Shop)
            return;

        if (GameInput.Instance.IsInteractPressed())
        {
            GameManager.Instance.OpenShopUI();
        }
    }
}