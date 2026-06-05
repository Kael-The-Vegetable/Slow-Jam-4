using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class PlayerStats : MonoBehaviour
{
    public float MaxHealth;
    public float Vulnerability;
    public double IFrames;

    public GameObject DieScreen;
    public GameObject deathParticles;
    
    private SpriteResolver m_Rarm;
    private SpriteResolver m_Larm;
    private SpriteResolver m_Rleg;
    private SpriteResolver m_Lleg;
    
    public float m_health;
    private double m_invuln;
    private float[] m_inventory = {0, 0, 0, 0};
    private bool m_ALR = true;
    private bool m_LLR = true;
    private bool m_isDead = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_health = MaxHealth;
        m_invuln = 0;
        m_isDead = false;
        // Get body parts
        m_Rarm = GameObject.Find("Tex_Template/root/body/chest/r_arm/RArmSlot").GetComponent<SpriteResolver>();
        m_Larm = GameObject.Find("Tex_Template/root/body/chest/l_arm/LArmSlot").GetComponent<SpriteResolver>();
        m_Rleg = GameObject.Find("Tex_Template/root/r_leg/RLegSlot").GetComponent<SpriteResolver>();
        m_Lleg = GameObject.Find("Tex_Template/root/l_leg/LLegSlot").GetComponent<SpriteResolver>();
        
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

    void OnPickup(string[] limb)
    {
        // Heal
        m_health += 5;
        
        // Get stat up value depending on label
        float limbstat = 0;
        if (limb[1] == "MC")
        {
            limbstat = 0;
        }
        else if (limb[1] == "Orc")
        {
            limbstat = 1;
        }
        else if (limb[1] == "Demon")
        {
            limbstat = 2;
        }
        Debug.Log($"Got {limb[1]} part");

        // Get which stat up depending on Category
        if (limb[0] == "Arm")
        {
            // If pickup was an arm...
            // Alternate replacing right then left
            if (m_ALR)
            {
                m_inventory[0] = limbstat;
                m_Rarm.SetCategoryAndLabel(limb[0], limb[1]);
                m_ALR = false;
            }
            else
            {
                m_inventory[1] = limbstat;
                m_Larm.SetCategoryAndLabel(limb[0], limb[1]);
                m_ALR = true;
            }
        }
        else if (limb[0] == "Leg")
        {
            // If pickup was a leg...
            // Alternate replacing right then left
            if (m_LLR)
            {
                m_inventory[2] = limbstat;
                m_Rleg.SetCategoryAndLabel(limb[0], limb[1]);
                m_LLR = false;
            }
            else
            {
                m_inventory[3] = limbstat;
                m_Lleg.SetCategoryAndLabel(limb[0], limb[1]);
                m_LLR = true;
            }
        }
        // Just heal if limb was neither

        // Update PlayerController
        SendMessage("OnEquip", m_inventory);
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

        // // Right arm
        // if (m_Rarm != null)
        // {
        //     switch (m_inventory[0])
        //     {
        //         case 0:
        //             // Default
        //             m_Rarm.SetCategoryAndLabel("Arm", "MC");
        //             break;
        //         case 1:
        //             // Orc
        //             m_Rarm.SetCategoryAndLabel("Arm", "Orc");
        //             break;
        //         case 2:
        //             // Demon
        //             m_Rarm.SetCategoryAndLabel("Arm", "Demon");
        //             break;
        //         default:
        //             m_Rarm.SetCategoryAndLabel("Arm", "Template");
        //             break;
        //     }
        // }
        
        // // Left arm
        // switch (m_inventory[1])
        // {
        //     case 0:
        //         // Default
        //         break;
        //     case 1:
        //         // Orc
        //         break;
        //     case 2:
        //         // Demon
        //         break;
        //     default:
        //         break;
        // }
        // // Right leg
        // switch (m_inventory[2])
        // {
        //     case 0:
        //         // Default
        //         break;
        //     case 1:
        //         // Orc
        //         break;
        //     case 2:
        //         // Demon
        //         break;
        //     default:
        //         break;
        // }
        // // Left leg
        // switch (m_inventory[3])
        // {
        //     case 0:
        //         // Default
        //         break;
        //     case 1:
        //         // Orc
        //         break;
        //     case 2:
        //         // Demon
        //         break;
        //     default:
        //         break;
        // }
        
        // Cap health at max
        if (m_health > MaxHealth)
        {
            m_health = MaxHealth;
        }

        // Decrement invulnerability timer every frame
        if (m_invuln > 0)
        {
            m_invuln -= 1;
        }
        
        // Disable die window UI element while alive
        if (m_health <= 0 && !m_isDead)
        {
            m_isDead = true;
            
            // Spawn death particles
            if (deathParticles != null)
            {
                GameObject particles = Instantiate(deathParticles, transform.position, Quaternion.identity);
                ParticleSystem ps = particles.GetComponent<ParticleSystem>();
                if (ps != null)
                    ps.Play();
                Destroy(particles, 1f);
            }
            
            // Play sound
            SFXManager.Instance.PlaySound(SFXManager.Instance.playerDeath);
            
            DieScreen.SetActive(true);
            Destroy(gameObject);
        }
        else if (m_health > 0)
        {
            // Activate die window when dead
            DieScreen.SetActive(false);
        }
    }
}