using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pauseImage;
    public bool isPaused = false;

    [SerializeField] private PlayerMovementGrappling playerMovement;
    [SerializeField] private Grappling playerGrap;
    [SerializeField] private PlayerCam camMovement;

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

            //
            playerMovement.enabled = false;
            playerGrap.enabled = false;
            camMovement.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            //
            playerMovement.enabled = true;
            playerGrap.enabled = true;
            camMovement.enabled = true;

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
