using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public float MaxHealth;
    
    private float m_health;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health = MaxHealth;
    }

    void OnHPUpdate(InputAction.CallbackContext context)
    {
        
    }

    // Updates 60 times per second regardless of framerate
    void FixedUpdate()
    {
        
    }
}
