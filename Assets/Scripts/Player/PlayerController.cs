using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerCon : MonoBehaviour
{
    // Variables
    private Vector2 m_moveVec;
    private Vector2 m_lookVec;
    private Animator m_animator;
    Rigidbody2D m_rigidbody;
    Collider2D m_collider;

    private GameObject m_interactable;
    private GameObject m_floor;

    // Vector that pulls player down when they are supposed to be grounded
    private Vector2 m_pull;
    
    // Gameplay Variables
    public float WalkSpeed = 5;
    public float JumpForce = 5;
    
    // Properties of the object used to check for groundedness
    public Transform GroundCheckTransform;
    public float GroundCheckRadius = 0.3f;
    public LayerMask LevelLayer;
    public LayerMask InteractableLayer;

    // Flags
    private bool m_grounded;
    private bool m_jumping;
    private bool m_canInteract;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
        m_animator = GetComponentInChildren<Animator>(); 
    }

    //------------------------------
    // Actions

    public void OnMove(InputAction.CallbackContext context)
    {
        // Update move vector
        m_moveVec = context.ReadValue<Vector2>();
        Debug.Log($"OnMove: {m_moveVec}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {   
        // Ignoring negative edge, If player is grounded...
        if(context.started && m_grounded)
        {
            // If down is being held...
            if(m_moveVec.y < 0)
            {
                // Don't Jump!
                // Get object that player is standing on
                m_floor = Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, LevelLayer).gameObject;
                
                // If player is standing on a SemiSolid...
                if (m_floor.CompareTag("SemiSolid"))
                {
                    // Clip through it
                    StartCoroutine(ClipThrough(m_floor.GetComponent<Collider2D>()));
                    Debug.Log($"Droped through {m_floor.name}");
                }
            }
            else
            {
                // Apply jumpforce to rigidbody and set jumping bool to true
                m_jumping = true;
                m_rigidbody.linearVelocityY = JumpForce;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Ignoring negative edge, If player can interact...
        if (context.started && m_canInteract)
        {
            // Gets gameobject of the interactible that groundcheck hitbox is colliding with
            m_interactable = Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, InteractableLayer).gameObject;
            Debug.Log($"Interacted with {m_interactable.name}");
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    //------------------------------
    // Core Routines
    private IEnumerator ClipThrough(Collider2D collider)
    {
        Physics2D.IgnoreCollision(m_collider, collider);
        m_rigidbody.linearVelocityY = 0-(JumpForce/2);
        yield return new WaitForSeconds(1f);
        Physics2D.IgnoreCollision(m_collider, collider, false);
    }

    //------------------------------
    // Update
    // Called 60 times per second regardless of framerate
    void FixedUpdate()
    {
        // Clears pull vector
        m_pull = Vector2.zero;
        // Checks if player is grounded
        m_grounded = Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, LevelLayer);

        // While player is falling...
        if((m_rigidbody.linearVelocityY <= 0))
        {
            // Sets jumping bool to false
            m_jumping = false;
        }
        
        // While player is grounded...
        if(m_grounded)
        {
            // Checks if player is at an interactable
            m_canInteract = Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, InteractableLayer);

            // If player is falling near the ground...
            if (!(m_jumping))
            {
                // Snaps player to the ground
                m_pull.y -= Physics2D.Raycast(GroundCheckTransform.position, Vector2.down, GroundCheckRadius, LevelLayer).distance;
                transform.Translate(m_pull);
            }
        }
        else
        {
            m_canInteract = false;
            m_floor = null;
        }

        // Applies movement to player
        m_rigidbody.linearVelocityX = m_moveVec.x * WalkSpeed;

        // Flip player sprites based on movement direction
        if (m_moveVec.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(m_moveVec.x), 1, 1);
        }

        // Update animator
        if (m_animator != null)
        {
            // Walking
            m_animator.SetBool("IsWalking", m_moveVec.x != 0);
        }
    }

    //------------------------------
    // Dev Functions

    // Draws hitboxes in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheckTransform.position, GroundCheckRadius);
    }
}
