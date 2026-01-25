using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Color fullColor = Color.white;
    [SerializeField] private Color emptyColor = Color.black;

    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].color = (i < currentHealth) ? fullColor : emptyColor;
        }
    }
}