using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneCharge : MonoBehaviour
{
    private bool SceneLoaded = false;

    AsyncOperation _asyncOperation;
    [SerializeField] int _sceneNumber;



    // Lista de paneles
    public List<GameObject> panels;
    // Índice del panel actual
    private int currentPanelIndex = 0;
    // Nombre de la escena a cargar al llegar al último panel





    private void Start()
    {
        StartCoroutine(AsyncCharge());


        // Asegúrate de que solo el primer panel esté activo al inicio
        for (int i = 0; i < panels.Count; i++)
        {
            panels[i].SetActive(i == 0);
        }

    }

    public void Update()
    {
        if (SceneLoaded && currentPanelIndex>1)
        {
            _asyncOperation.allowSceneActivation = true;
        }
    }

    public void OnNextButtonPressed()
    {
        // Desactiva el panel actual
        panels[currentPanelIndex].SetActive(false);

        // Incrementa el índice del panel actual
        currentPanelIndex++;

        // Verifica si hemos llegado al último panel
        if (currentPanelIndex < panels.Count)
        {
            panels[currentPanelIndex].SetActive(true);

        }
        else
        {
            Debug.Log("corregir");
            // Activa el siguiente panel
        }
    }


    IEnumerator AsyncCharge()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(_sceneNumber);
        _asyncOperation.allowSceneActivation = false;
        Application.backgroundLoadingPriority = ThreadPriority.High;

        while (!_asyncOperation.isDone)
        {
            // La propiedad 'progress' va de 0 a 0.9. Multiplicamos por 100 para tener el porcentaje.
            float progressPercentage = _asyncOperation.progress * 100;
            Debug.Log("Carga de la escena: " + progressPercentage + "%");

            // Al llegar al 90% la carga está completa y espera activación.
            if (_asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Carga completa. Esperando activación...");
                SceneLoaded = true;


                break; // Sale del bucle porque ya está cargado.
            }
        }

        yield return null;

    }






}