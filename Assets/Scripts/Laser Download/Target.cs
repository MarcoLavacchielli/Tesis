using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    public void Hit()
    {
        Debug.Log("Target Hit " + name);

        // Reiniciar la escena actual
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
