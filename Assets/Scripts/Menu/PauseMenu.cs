using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pauseImage;
    public bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
            pauseCanvas.SetActive(true);
            pauseImage.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
            pauseCanvas.SetActive(false);
            pauseImage.SetActive(false);
        }
    }

    public void Loadscene(string scenename)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scenename);
    }

    public void Restart()
    {
        Time.timeScale = 1f;  
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
