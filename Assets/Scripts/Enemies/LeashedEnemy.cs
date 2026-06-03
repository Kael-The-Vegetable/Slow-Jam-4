using UnityEngine;
using System.Collections;

public class LeashedEnemy : MonoBehaviour
{
    // Components
    private Rigidbody2D m_rb;
    private Collider2D m_col;
    
    // Parameters
    public Transform Home;
    public float WalkSpeed;
    public float LeashMax;
    public float MaxHP;

    // Variables
    private Vector2 m_moveVec;
    private float m_leash;
    private bool m_wait;
    private bool m_stun;
    private float m_hp;
    
    // ------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_col = GetComponent<Collider2D>();
        m_wait = true;
        m_moveVec.x = 1;
        m_hp = MaxHP;
    }

    // ------------------------------
    // Functions

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "HitBox")
        {
            m_stun = true;
            m_hp -= 1;
            m_rb.linearVelocity = col.collider.transform.position - m_col.transform.position;
        }
    }

    void OnHurt(float dmg)
    {
        m_stun = true;
        m_hp -= dmg;
        m_rb.linearVelocityY = 2f;
        StartCoroutine(Hurt(0.25f));
    }

    // ------------------------------
    // Core Routines
    private IEnumerator Pause(float t)
    {
        // Sets wait bool to false after t seconds
        yield return new WaitForSeconds(t);
        m_wait = false;
    }

    private IEnumerator Walk(float t)
    {
        // Sets wait bool to true after t seconds
        yield return new WaitForSeconds(t);
        m_wait = true;
    }

    private IEnumerator Hurt(float t)
    {
        m_wait = true;
        yield return new WaitForSeconds(t);
        m_stun = false;
    }

    // ------------------------------
    // Called 60 times per second regardless of framerate
    void FixedUpdate()
    {
        if (m_hp <= 0)
        {
            Destroy(gameObject);
        }

        // Checks x distance from Home point
        m_leash = Home.position.x - m_rb.position.x;
        
        // Goes towards home when too far from it
        if (m_leash >= LeashMax)
        {
            m_moveVec.x = 1;
        }
        if (m_leash <= 0 - LeashMax)
        {
            m_moveVec.x = -1;
        }

        if (m_stun)
        {
            m_rb.linearVelocityX = 0;
        }
        else if (m_wait)
        {
            // Stops
            m_rb.linearVelocityX = 0;
            // Waits for a second
            StartCoroutine(Pause(1f));
            // Starts walking for the next few seconds
            StartCoroutine(Walk(3f));
        }
        else
        {
            // Moves when not waiting
            m_rb.linearVelocityX = m_moveVec.x * WalkSpeed;
        }
    }
}
