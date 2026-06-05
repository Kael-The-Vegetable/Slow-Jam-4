using UnityEngine;
using UnityEngine.U2D.Animation;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public Sprite itemSprite;
    public string Category = "Arm";
    public string Label = "Demon";
    
    private SpriteResolver m_sr;
    private string[] m_limb;
    
    void Start()
    {   
        m_limb = new string[] {Category, Label};

        m_sr = GetComponent<SpriteResolver>();
        if (m_sr != null)
        {
            m_sr.SetCategoryAndLabel(Category, Label);
        }
    }

    public void OnDrop(string[] limb)
    {
        Category = limb[0];
        Label = limb[1];
        Debug.Log($"{Category} {Label}");
        m_limb = new string[] {Category, Label};
        m_sr.SetCategoryAndLabel(Category, Label);
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