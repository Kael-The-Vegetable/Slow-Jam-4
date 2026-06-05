using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    private TMP_Text statsText;
    private GameObject player;
    private PlayerStats playerStats;
    private PlayerCon playerCon;
    
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
            {
                playerStats = player.GetComponent<PlayerStats>();
                playerCon = player.GetComponent<PlayerCon>();
            }
        }
        
        if (playerStats != null && playerCon != null && statsText != null)
        {
            float displaySpeed = playerCon.WalkSpeed - 7f;
            statsText.SetText($"HP: {playerStats.m_health}/{playerStats.MaxHealth}\nATK: {playerCon.Atk}\nSPD: {displaySpeed}");
        }
    }
}