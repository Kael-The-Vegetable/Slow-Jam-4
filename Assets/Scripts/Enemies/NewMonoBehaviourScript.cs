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

    // Variables
    private Vector2 m_moveVec;
    private float m_leash;
    private bool m_LorR;
    private float m_walktimer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_col = GetComponent<Collider2D>();
        m_walktimer = 180f;
    }

    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(1f);
    }

    // Called 60 times per second regardless of framerate
    void FixedUpdate()
    {
        m_leash = Vector2.Distance(Home.position, m_rb.position);

        if ((Home.position.x - m_rb.position.x) >= 0)
        {
            m_moveVec.x = WalkSpeed;
        }
        else
        {
            m_moveVec.x = 0 - WalkSpeed;
        }

        if (m_walktimer > 0)
        {
            m_walktimer -= 1;
            m_rb.linearVelocityX = m_moveVec.x;
        }
        else
        {
            m_walktimer = 180f;
            StartCoroutine(Pause());
        }
    }
}
