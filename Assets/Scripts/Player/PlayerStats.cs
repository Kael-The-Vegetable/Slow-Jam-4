using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public float MaxHealth;
    public float Vulnerability;
    public double IFrames;

    public GameObject DieScreen;
    private SpriteRenderer m_Rarm;
    private SpriteRenderer m_Larm;
    private SpriteRenderer m_Rleg;
    private SpriteRenderer m_Lleg;
    
    private float m_health;
    private double m_invuln;
    private float[] m_inventory = {0, 0, 0, 0};
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health = MaxHealth;
        m_invuln = 0;
        // Get body parts
        m_Rarm = GameObject.Find("Char_Template/Tex_Template/RArm").GetComponent<SpriteRenderer>();
        m_Larm = GameObject.Find("Char_Template/Tex_Template/LArm").GetComponent<SpriteRenderer>();
        m_Rleg = GameObject.Find("Char_Template/Tex_Template/RLeg").GetComponent<SpriteRenderer>();
        m_Lleg = GameObject.Find("Char_Template/Tex_Template/LLeg").GetComponent<SpriteRenderer>();
    }

    void OnHurt(float dir)
    {
        if (dir < 0)
        {
            m_health += dir;
        }
        else
        {
            m_health -= dir;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Hazard") && m_invuln <= 0)
        {
            m_invuln = IFrames;
            float dir = transform.position.x - collision.transform.position.x;
            if (dir >= 0)
            {
                dir = 1f;
            }
            else
            {
                dir = -1f;
            }
            SendMessage("OnHurt", dir);
        }
    }

    // Updates 60 times per second regardless of framerate
    void FixedUpdate()
    {
        // Sprite swapping according to equiped parts
        // Right arm
        switch (m_inventory[0])
        {
            case 0:
                // Default
                break;
            case 1:
                // Orc
                break;
            case 2:
                // Demon
                break;
            default:
                break;
        }
        // Left arm
        switch (m_inventory[1])
        {
            case 0:
                // Default
                break;
            case 1:
                // Orc
                break;
            case 2:
                // Demon
                break;
            default:
                break;
        }
        // Right leg
        switch (m_inventory[2])
        {
            case 0:
                // Default
                break;
            case 1:
                // Orc
                break;
            case 2:
                // Demon
                break;
            default:
                break;
        }
        // Left leg
        switch (m_inventory[3])
        {
            case 0:
                // Default
                break;
            case 1:
                // Orc
                break;
            case 2:
                // Demon
                break;
            default:
                break;
        }
        
        // Decrement invulnerability timer every frame
        if (m_invuln > 0)
        {
            m_invuln -= 1;
        }
        
        // Disable die window UI element while alive
        if (m_health <= 0)
        {
            Destroy(gameObject);
            DieScreen.SetActive(true);
        }
        else
        {
            // Activate die window when dead
            DieScreen.SetActive(false);
        }
    }
}
