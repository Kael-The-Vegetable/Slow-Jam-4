using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [Header("Item Settings")]
    public string statToIncrease = "Health";
    public float increaseAmount = 1f;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Picked up {statToIncrease} +{increaseAmount}");
            Destroy(gameObject);
        }
    }
}