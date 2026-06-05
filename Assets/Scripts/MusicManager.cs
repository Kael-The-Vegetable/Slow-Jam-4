using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        
        if (currentScene == "MainMenu" || currentScene == "Victory")
        {
            Destroy(gameObject);
            Instance = null;
        }
    }
}