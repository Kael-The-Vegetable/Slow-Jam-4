using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    private AudioSource source;

    [Header("SFX")]
    public AudioClip playerJump;
    public AudioClip playerSlash;
    public AudioClip playerHit;
    public AudioClip playerDeath;
    public AudioClip enemySlash;
    public AudioClip enemyHit;
    public AudioClip enemyDeath;
    public AudioClip upgrade;
    public AudioClip door;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            source = GetComponent<AudioSource>();
            if (source == null) source = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            source.PlayOneShot(clip);
    }
}