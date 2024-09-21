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
    // �ndice del panel actual
    private int currentPanelIndex = 0;
    // Nombre de la escena a cargar al llegar al �ltimo panel





    private void Start()
    {
        StartCoroutine(AsyncCharge());


        // Aseg�rate de que solo el primer panel est� activo al inicio
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

        // Incrementa el �ndice del panel actual
        currentPanelIndex++;

        // Verifica si hemos llegado al �ltimo panel
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

            // Al llegar al 90% la carga est� completa y espera activaci�n.
            if (_asyncOperation.progress >= 0.9f)
            {
                Debug.Log("Carga completa. Esperando activaci�n...");
                SceneLoaded = true;


                break; // Sale del bucle porque ya est� cargado.
            }
        }

        yield return null;

    }






}