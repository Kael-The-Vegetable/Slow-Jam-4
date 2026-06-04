using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Door : MonoBehaviour
{
    private Animator animator;
    private GameObject player;
    private InputAction interactAction;
    private bool playerInside = false;
    private bool isTransitioning = false;
    private float fadeDelay = 1.5f;
    
    [SerializeField] private string sceneToLoad = "NextScene";
    [SerializeField] private Animator fadeAnimator;
    
    void Start()
    {
        animator = GetComponent<Animator>();

        interactAction = InputSystem.actions.FindAction("Interact");
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
        if (playerInside && interactAction.WasPressedThisFrame() && !isTransitioning)
        {
            isTransitioning = true;
            
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
}