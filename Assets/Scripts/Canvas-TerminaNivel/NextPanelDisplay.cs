using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para la gestión de escenas

public class NextPanelDisplay : MonoBehaviour
{
    // Lista de paneles
    public List<GameObject> panels;
    // Índice del panel actual
    private int currentPanelIndex = 0;
    // Nombre de la escena a cargar al llegar al último panel
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        // Asegúrate de que solo el primer panel esté activo al inicio
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == 0);
        }
    }

    // Función que se llamará al presionar el botón
    public void OnNextButtonPressed()
    {
        // Desactiva el panel actual
        panels[currentPanelIndex].SetActive(false);

        // Incrementa el índice del panel actual
        currentPanelIndex++;

        // Verifica si hemos llegado al último panel
        if (currentPanelIndex >= panels.Count)
        {
            // Carga la nueva escena
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            // Activa el siguiente panel
            panels[currentPanelIndex].SetActive(true);
        }
    }
}
