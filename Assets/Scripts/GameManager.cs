using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static int waveNumber;

    private void Awake()
    {
        Instance = this;
        waveNumber = 1;
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    public void IncreaseWaveNumber()
    {
        waveNumber++;
    }
}
