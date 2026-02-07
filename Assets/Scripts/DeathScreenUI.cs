using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    private PlayerHealth playerHealth;

    private void Start()
    {
        restartButton.onClick.AddListener(Restart);

        playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied += OnPlayerDied;
        }
        else
        {
            Debug.LogError("DeathScreenUI: PlayerHealth not found");
        }

        gameObject.SetActive(false);
    }

    private void OnPlayerDied(object sender, EventArgs e)
    {
        Debug.Log("DeathScreenUI received death event");
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        restartButton.onClick.RemoveListener(Restart);

        if (playerHealth != null)
            playerHealth.OnPlayerDied -= OnPlayerDied;
    }
}
