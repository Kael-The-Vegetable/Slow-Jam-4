using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    private TMP_Text statsText;
    private GameObject player;
    private PlayerStats playerStats;
    
    void Start()
    {
        statsText = GetComponent<TMP_Text>();
    }
    
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerStats = player.GetComponent<PlayerStats>();
        }
        
        if (playerStats != null && statsText != null)
        {
            statsText.SetText($"HP: {playerStats.m_health}/{playerStats.MaxHealth}");
        }
    }
}

// use this once the stats are available
// statsText.SetText($"HP: {playerStats.m_health}/{playerStats.MaxHealth}\nATK: {playerStats.attack}\nDEF: {playerStats.defense}\nSPD: {playerStats.speed}");