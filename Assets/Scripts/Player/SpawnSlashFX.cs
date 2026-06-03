using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
    [SerializeField] public GameObject slashVFXPrefab;
    [SerializeField] public Transform spawnPoint;
    
    public void SpawnSlashVFX(float facingDirection)
    {
        if (slashVFXPrefab == null) return;
        
        // Spawn the vfx prefab
        GameObject vfx = Instantiate(slashVFXPrefab, spawnPoint.position, Quaternion.identity);
        
        // Flip the spawned VFX based on facing direction
        vfx.transform.localScale = new Vector3(facingDirection, 1, 1);
        
        // Destroy prefab after animation is complete
        Animator vfxAnimator = vfx.GetComponent<Animator>();
        if (vfxAnimator != null)
        {
            float animLength = vfxAnimator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(vfx, animLength);
        }
        else
        {
            Destroy(vfx, 0.5f);
        }
    }
}