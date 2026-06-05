using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    private TMP_Text hpIndicator;
    private GameObject player;
    private PlayerStats playerStats;
    
    void Start()
    {
        hpIndicator = GetComponent<TMP_Text>();
    }
    
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerStats = player.GetComponent<PlayerStats>();
        }
        
        if (playerStats != null && hpIndicator != null)
        {
            hpIndicator.SetText($"HP: {playerStats.m_health}/{playerStats.MaxHealth}");
        }
    }
}