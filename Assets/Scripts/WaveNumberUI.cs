using TMPro;
using UnityEngine;

public class WaveNumberUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveNumberText;


    private void Update()
    {
     waveNumberText.text = "Wave: " + GameManager.Instance.GetWaveNumber();   
    }
}
