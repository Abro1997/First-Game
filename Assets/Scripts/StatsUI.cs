using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statsTextMesh;

    private void Start()
    {
        UpdateStatsUI();
        PlayerStats.Instance.OnStatsChanged += PlayerStats_OnStatsChanged;
    }
    private void PlayerStats_OnStatsChanged(object sender, System.EventArgs e)
    {
        UpdateStatsUI();
    }
    private void UpdateStatsUI()
    {
        PlayerStats stats = PlayerStats.Instance;
        statsTextMesh.text = $"Damage: {stats.GetDamage()}\n" +
                             $"Move Speed: {stats.GetMoveSpeed()}\n" +
                             $"Fire Rate: {stats.GetFireRate():0.00}\n";
    }
}
