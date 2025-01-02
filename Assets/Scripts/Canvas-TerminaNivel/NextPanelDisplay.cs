using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para la gesti�n de escenas

public class NextPanelDisplay : MonoBehaviour
{
    // Lista de paneles
    public List<GameObject> panels;
    // �ndice del panel actual
    private int currentPanelIndex = 0;
    // Nombre de la escena a cargar al llegar al �ltimo panel
    public string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        // Aseg�rate de que solo el primer panel est� activo al inicio
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == 0);
        }
    }

    // Funci�n que se llamar� al presionar el bot�n
    public void OnNextButtonPressed()
    {
        // Desactiva el panel actual
        panels[currentPanelIndex].SetActive(false);

        // Incrementa el �ndice del panel actual
        currentPanelIndex++;

        // Verifica si hemos llegado al �ltimo panel
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
