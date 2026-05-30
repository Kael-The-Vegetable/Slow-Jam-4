using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerCon : MonoBehaviour
{
    // Variables
    private Vector2 m_moveMag;
    private Vector2 m_lookMag;
    private Animator m_animator;
    Rigidbody2D m_rigidbody;

    // Vector that pulls player down when they are supposed to be grounded
    private Vector2 m_pull;
    
    // Gameplay Variables
    public float WalkSpeed = 5;
    public float JumpForce = 5;
    
    // Properties of the object used to check for groundedness
    public Transform GroundCheckTransform;
    public float GroundCheckRadius;
    private RaycastHit2D m_groundedRay;
    public LayerMask LevelLayer;

    // Flags
    private bool m_grounded;
    private bool m_jumping;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_moveMag = context.ReadValue<Vector2>();
        Debug.Log($"OnMove: {m_moveMag}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {   
        if(context.started && m_grounded)
        {
            m_jumping = true;
            m_rigidbody.linearVelocityY = JumpForce;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // Clears pull vector
        m_pull = Vector2.zero;
        // Checks if player is grounded
        m_grounded = Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, LevelLayer);

        // Checks if player is falling
        if((m_rigidbody.linearVelocityY <= 0))
        {
            m_jumping = false;
        }
        
        // Snaps player to ground when falling near the ground
        if(!(m_jumping) && m_grounded)
        {
            m_groundedRay = Physics2D.Raycast(GroundCheckTransform.position, Vector2.down, GroundCheckRadius, LevelLayer);
            m_pull.y -= m_groundedRay.distance;
            transform.Translate(m_pull);
        }

        // Applies movement to player
        m_rigidbody.linearVelocityX = m_moveMag.x * WalkSpeed;

        // Flips player sprites
        if (m_moveMag.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(m_moveMag.x), 1, 1);
        }

        // For animator
        m_animator.SetBool("IsWalking", m_moveMag.x != 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheckTransform.position, GroundCheckRadius);
    }
}
