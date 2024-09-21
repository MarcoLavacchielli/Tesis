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

    private void Start()
    {
        StartCoroutine(AsyncCharge());
    }


    IEnumerator AsyncCharge()
    {
        _asyncOperation = SceneManager.LoadSceneAsync(_sceneNumber);
        _asyncOperation.allowSceneActivation = false;
        Application.backgroundLoadingPriority = ThreadPriority.Normal;

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

    public void ChangeSceneThroughAsync()
    {
        if (SceneLoaded)
        {
            _asyncOperation.allowSceneActivation = true;

        }
        else
        {
            Debug.Log("aun no cargo la escena");
        }
    }




}