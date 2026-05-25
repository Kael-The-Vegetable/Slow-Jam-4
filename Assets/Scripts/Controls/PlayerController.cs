using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCon : MonoBehaviour
{
    private Vector2 m_moveMag;
    private Vector2 m_lookMag;
    private Animator m_animator;
    Rigidbody2D m_rigidbody;
    BoxCollider2D m_groundCheck;
    

    public float WalkSpeed = 5;
    public float JumpForce = 5;

    private bool m_grounded;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_groundCheck = GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D m_groundCheck)
    {
        m_grounded = true;
    }

    void OnTriggerExit2D(Collider2D m_groundCheck)
    {
        m_grounded = false;
        Debug.Log("Airborn");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_moveMag = context.ReadValue<Vector2>();
        Debug.Log($"OnMove: {m_moveMag}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {   
        Debug.Log($"OnMove: {context.ReadValue<float>()}");
        if (m_grounded)
        {
            m_rigidbody.linearVelocityY = JumpForce;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        m_rigidbody.linearVelocityX = m_moveMag.x * WalkSpeed;
    }
}
