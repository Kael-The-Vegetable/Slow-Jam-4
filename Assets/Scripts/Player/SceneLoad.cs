using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField] public GameObject mainMenuCanvas;
    [SerializeField] public GameObject tutorialPanel;

    public void ShowTutorial()
    {
        mainMenuCanvas.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialPanel.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(sceneName);
    }
}