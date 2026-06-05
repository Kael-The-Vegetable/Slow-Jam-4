using UnityEngine;
using UnityEngine.U2D.Animation;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public Sprite itemSprite;
    public string Category = "Arm";
    public string Label = "Demon";
    public string statToIncrease = "Health";
    public float increaseAmount = 10f;
    
    private SpriteRenderer spriteRenderer;
    private SpriteResolver m_sr;
    private string[] m_limb;
    
    void Start()
    {
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // if (spriteRenderer != null && itemSprite != null)
        // {
        //     spriteRenderer.sprite = itemSprite;
        // }
        
        m_limb = new string[] {Category, Label};

        m_sr = GetComponent<SpriteResolver>();
        if (m_sr != null)
        {
            m_sr.SetCategoryAndLabel(Category, Label);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log($"Picked up {statToIncrease} +{increaseAmount}");
            other.SendMessage("OnPickup", m_limb);
            Destroy(gameObject);
        }
    }
}