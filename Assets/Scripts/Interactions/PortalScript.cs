using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PortalScript : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private InputAction interactAction;
    private bool playerInside = false;
    private bool isTransitioning = false;
    private bool isUnlocked = false;
    private float fadeDelay = 1.5f;
    private int enemiesRemaining = 0;
    
    [SerializeField] private string sceneToLoad = "NextScene";
    [SerializeField] private Animator fadeAnimator;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        
        // Find SpriteRenderer on the "DoorFront"
        Transform doorFront = transform.Find("DoorFront");
        if (doorFront != null)
        {
            spriteRenderer = doorFront.GetComponent<SpriteRenderer>();
        }
        
        interactAction = InputSystem.actions.FindAction("Interact");

        SetDoorGray(true);
        isUnlocked = false;
        TrackEnemies();
    }
    
    void TrackEnemies()
    {
        LeashedEnemy[] enemies = FindObjectsByType<LeashedEnemy>(FindObjectsInactive.Exclude);
        enemiesRemaining = enemies.Length;
        
        foreach (LeashedEnemy enemy in enemies)
        {
            enemy.OnEnemyDied += HandleEnemyDeath;
        }
        
        if (enemiesRemaining == 0)
        {
            UnlockDoor();
        }
    }
    
    void HandleEnemyDeath()
    {
        enemiesRemaining--;
        Debug.Log($"Enemies remaining: {enemiesRemaining}");
        
        if (enemiesRemaining <= 0 && !isUnlocked)
        {
            UnlockDoor();
        }
    }
    
    void UnlockDoor()
    {
        isUnlocked = true;
        SetDoorGray(false);
    }
    
    void SetDoorGray(bool gray)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = gray ? Color.gray : Color.white;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.gameObject;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }
    
    void Update()
    {
        if (playerInside && interactAction.WasPressedThisFrame() && !isTransitioning && isUnlocked)
        {
            isTransitioning = true;
            // Play sound
            SFXManager.Instance.PlaySound(SFXManager.Instance.door);

            if (animator != null)
                animator.SetTrigger("Open");
            
            if (fadeAnimator != null)
                fadeAnimator.SetTrigger("Fade");
            
            Invoke(nameof(LoadScene), fadeDelay);
        }
    }
    
    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    
    void OnDestroy()
    {
        LeashedEnemy[] enemies = FindObjectsByType<LeashedEnemy>(FindObjectsInactive.Exclude);
        foreach (LeashedEnemy enemy in enemies)
        {
            enemy.OnEnemyDied -= HandleEnemyDeath;
        }
    }
}