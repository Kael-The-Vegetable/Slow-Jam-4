using UnityEngine;
using System.Collections;

public class LeashedEnemy : MonoBehaviour
{
    // Components
    private Rigidbody2D m_rb;
    private Collider2D m_col;
    private Animator m_anim;
    private Collider2D m_target;
    
    // Parameters
    public Transform Home;
    public float WalkSpeed = 4f;
    public float LeashMax = 5f;
    public float MaxHP = 3f;
    public float Damage = 1f;
    public Transform AttackTransform;
    public float AttackRadius = 1.3f;
    public GameObject slashFX;
    public LayerMask EntityLayer;

    // Variables
    private Vector2 m_moveVec;
    private float m_leash;
    private bool m_wait;
    private bool m_acting;
    private bool m_hitting;
    private float m_hp;
    
    // Death event for door to listen to
    public System.Action OnEnemyDied;
    
    // ------------------------------
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_col = GetComponent<Collider2D>();
        m_anim = GetComponentInChildren<Animator>();
        m_wait = true;
        m_moveVec.x = 1;
        m_hp = MaxHP;
    }

    // ------------------------------
    // Methods

    void OnHit(float dmg)
    {
        m_hp -= dmg;
        // Knockback
        m_rb.linearVelocityY = 4f;
        m_acting = true;
        StartCoroutine(Hurt(0.25f));
    }

    public void OnAttack()
    {
        m_acting = true;
        StartCoroutine(Attack());            

        // Spawn slashFX
        GameObject vfx = Instantiate(slashFX, AttackTransform.position, Quaternion.identity);
        // Play sound
        SFXManager.Instance.PlaySound(SFXManager.Instance.enemySlash);
        // Flip it according to player facing direction
        vfx.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
        // Despawn slashFX
        Destroy(vfx, 0.3f);
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
        // Sets acting bool to false after t seconds
        // Play sound
        SFXManager.Instance.PlaySound(SFXManager.Instance.enemyHit);
        m_anim.SetTrigger("Hurting");
        yield return new WaitForSeconds(t);
        m_acting = false;
    }

    private IEnumerator Attack()
    {
        m_anim.SetTrigger("Attacking");
        // Startup
        yield return new WaitForSeconds(0.1f);
        m_hitting = true;
        // Hitbox is active
        yield return new WaitForSeconds(0.3f);
        m_hitting = false;
        // End lag
        yield return new WaitForSeconds(0.1f);
        // Total seconds waited is duration of attack animation
        m_acting = false;
    }

    private void Die()
    {
        // Play sound
        SFXManager.Instance.PlaySound(SFXManager.Instance.enemyDeath);
        // Tell the door that this enemy died
        OnEnemyDied?.Invoke();
        Destroy(gameObject);
    }

    // ------------------------------
    // Called 60 times per second regardless of framerate
    void FixedUpdate()
    {
        if (m_hp <= 0)
        {
            Die();
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

        if (m_acting)
        {
            // Freeze x position when acting
            m_rb.linearVelocityX = 0;
            
            // While attack hitbox is active...
            if (m_hitting)
            {
                // If an entity was hit...
                m_target = Physics2D.OverlapCircle(AttackTransform.position, AttackRadius, EntityLayer);
                if (m_target)
                {
                    float dir = transform.position.x - m_target.transform.position.x;
                    if (dir >= 0)
                    {
                        dir = 1f;
                    }
                    else
                    {
                        dir = -1f;
                    }
                    // Tell entity to do OnHurt
                    m_target.gameObject.SendMessage("OnHurt", dir * Damage);
                    // Clear target variable
                    m_target = null;
                    // Disable hitbox so only one entity can be hit per hitbox
                    m_hitting = false;
                }
            }
        
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

        if (!m_acting)
        {
            m_target = Physics2D.OverlapCircle(AttackTransform.position, AttackRadius, EntityLayer);
            if(m_target && m_target.gameObject.tag == "Player")
            {
                OnAttack();
            }
        }

        // Flip sprites based on movement direction
        if (m_rb.linearVelocityX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(m_moveVec.x), 1, 1);
        }

        if (m_anim)
        {
            m_anim.SetBool("IsActing", m_acting);
            m_anim.SetBool("IsIdle", m_wait);
            m_anim.SetBool("IsWalking", !m_wait);
            m_anim.SetBool("IsFalling", m_rb.linearVelocityY < 0);
        }
    }

    //------------------------------
    // Dev Functions

    // Draws hitboxes in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackTransform.position, AttackRadius);
    }
}