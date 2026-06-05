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
            statsText.SetText($"Health: {playerStats.m_health}/{playerStats.MaxHealth}");
        }
    }
}

// use this once the stats are available
// statsText.SetText($"Health: {playerStats.m_health}/{playerStats.MaxHealth}\nAttack: {playerStats.attack}\nDefense: {playerStats.defense}\nSpeed: {playerStats.speed}");