using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Escape : MonoBehaviour
{
    private bool isPlayerInTrigger = false;

    public TvTurnOffEffect effect;
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            effect.TriggerTVEffect();

            // Inicia la corrutina para esperar 3 segundos antes de cargar la escena
            StartCoroutine(WaitAndLoadScene());
        }
    }

    private IEnumerator WaitAndLoadScene()
    {
        // Espera 3 segundos
        yield return new WaitForSeconds(3f);

        // Carga la escena después del retraso
        SceneManager.LoadScene(sceneName);
    }
}
