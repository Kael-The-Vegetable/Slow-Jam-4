using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public float MaxHealth;
    public float Vulnerability;
    public double IFrames;

    public GameObject DieScreen;
    
    private float m_health;
    private double m_invuln;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health = MaxHealth;
        m_invuln = 0;
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
        if (m_invuln > 0)
        {
            m_invuln -= 1;
        }
        if (m_health <= 0)
        {
            Destroy(gameObject);
            DieScreen.SetActive(true);
        }
        else
        {
            DieScreen.SetActive(false);
        }
    }
}
