using UnityEngine;

public class PortalScript : MonoBehaviour
{
    private Animator m_animator;

    public Transform Destination;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Called 60 times per second regardless of framerate
    void FixedUpdate()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, Destination.position);
    }
}
