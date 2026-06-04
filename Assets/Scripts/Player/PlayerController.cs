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
    private Collider2D m_target;

    // Vector that pulls player down when they are supposed to be grounded
    private Vector2 m_pull;
    
    // Gameplay Variables
    public float WalkSpeed = 5;
    public float JumpForce = 5;
    public float Atk = 1;
    
    // Properties of objects used to check for groundedness or attack
    public Transform GroundCheckTransform;
    public float GroundCheckRadius = 0.3f;
    public Transform AttackTransform;
    public float AttackRadius;
    public GameObject slashFX;

    public LayerMask LevelLayer;
    public LayerMask InteractableLayer;
    public LayerMask EntityLayer;

    // Flags
    private bool m_grounded;
    private bool m_jumping;
    private bool m_canInteract;
    private bool m_animating;
    private bool m_hitting;
    
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
        //Debug.Log($"OnMove: {m_moveVec}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {   
        // Ignoring negative edge, If player is grounded...
        if (context.started && m_grounded && !(m_animating))
        {
            // If down is being held...
            if (m_moveVec.y < 0)
            {
                // Don't Jump!
                // Get object that player is standing on
                m_floor = Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, LevelLayer).gameObject;
                
                // If player is standing on a SemiSolid...
                if (m_floor.CompareTag("SemiSolid"))
                {
                    // Clip through it
                    StartCoroutine(ClipThrough(m_floor.GetComponent<Collider2D>()));
                    //Debug.Log($"Droped through {m_floor.name}");
                }
            }
            else
            {
                // Apply jumpforce to rigidbody and set jumping bool to true
                m_jumping = true;
                m_rigidbody.linearVelocityY = JumpForce;
            }
        }

        if (context.canceled && m_jumping)
        {
            m_rigidbody.linearVelocityY = m_rigidbody.linearVelocityY/2;
            //m_rigidbody.linearVelocityY = -0.5f;
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
        if (context.started && !(m_animating))
        {
            m_animator.SetTrigger("Attacking");
            StartCoroutine(Attack());            

            // Spawn slashFX
            GameObject vfx = Instantiate(slashFX, AttackTransform.position, Quaternion.identity);
            // Flip it according to player facing direction
            vfx.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
            // Despawn slashFX
            Destroy(vfx, 0.3f);
        }
    }

    public void OnHurt(float dir)
    {
        m_animator.SetTrigger("Hurt");
        // Knocks Back
        m_rigidbody.linearVelocityY = 4f;
        m_rigidbody.linearVelocityX = 4f * dir;
        StartCoroutine(Hurt());
    }

    //------------------------------
    // Core Routines
    private IEnumerator Hurt()
    {
        // Play sound
        SFXManager.Instance.PlaySound(SFXManager.Instance.playerHit);
        m_animator.SetBool("IsActing", true);
        yield return new WaitForSeconds(0.333f);
        m_animator.SetBool("IsActing", false);
    }
    
    private IEnumerator Jump()
    {
        m_animator.SetBool("IsActing", true);
        yield return new WaitForSeconds(0.25f);
        m_animator.SetBool("IsActing", false);
    }
    
    private IEnumerator ClipThrough(Collider2D collider)
    {
        m_animator.SetBool("IsActing", true);
        Physics2D.IgnoreCollision(m_collider, collider);
        m_rigidbody.linearVelocityY = 0-(JumpForce/2);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(m_collider, collider, false);
        m_animator.SetBool("IsActing", false);
    }

    private IEnumerator Attack()
    {
        // Play sound
        SFXManager.Instance.PlaySound(SFXManager.Instance.playerSlash);
        m_animator.SetBool("IsActing", true);
        // Startup
        yield return new WaitForSeconds(0.1f);
        m_hitting = true;
        // Hitbox is active
        yield return new WaitForSeconds(0.3f);
        m_hitting = false;
        // End lag
        yield return new WaitForSeconds(0.1f);
        // Total seconds waited is durration of attack animation
        m_animator.SetBool("IsActing", false);
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
        // Checks if player is in an un-interupible animation
        m_animating = m_animator.GetBool("IsActing");

        // While attack hitbox is active...
        if (m_hitting)
        {
            // If an entity was hit...
            m_target = Physics2D.OverlapCircle(AttackTransform.position, AttackRadius, EntityLayer);
            if (m_target)
            {
                // Tell the entity to do OnHit
                m_target.gameObject.SendMessage("OnHit", Atk);
                // Clear target variable
                m_target = null;
                // Disable hitbox so only one entity can be hit per hitbox
                m_hitting = false;
            }
        }

        // While player is falling...
        if (m_rigidbody.linearVelocityY <= 0)
        {
            // Sets jumping bool to false
            m_jumping = false;
        }
        
        // While player is grounded...
        if (m_grounded)
        {
            // Checks if player is at an interactable
            m_canInteract = Physics2D.OverlapCircle(GroundCheckTransform.position, GroundCheckRadius, InteractableLayer);

            // If player is falling near the ground...
            if (!(m_jumping) && !(m_animating))
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
        if (m_animating)
        {
            // Halves player movement speed while acting
            m_rigidbody.linearVelocityX = m_moveVec.x * (WalkSpeed/2);
        }
        else
        {
            // Full speed otherwise
            m_rigidbody.linearVelocityX = m_moveVec.x * WalkSpeed;
        }
        
        // Flip player sprites based on movement direction
        if (m_moveVec.x != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(m_moveVec.x), 1, 1);
        }

        // Update animator
        if (m_animator != null)
        {
            // Walking
            m_animator.SetBool("IsWalking", (m_moveVec.x != 0) && m_grounded);
            // Jumping
            m_animator.SetBool("IsJumping", m_jumping);
            // Falling
            m_animator.SetBool("IsFalling", !(m_jumping) && !(m_grounded));
            
        }
    }

    //------------------------------
    // Dev Functions

    // Draws hitboxes in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheckTransform.position, GroundCheckRadius);
        Gizmos.DrawWireSphere(AttackTransform.position, AttackRadius);
    }
}
