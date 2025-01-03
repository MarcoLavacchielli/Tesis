using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{

    [SerializeField] private JsonSaveGameManager saveGameManager;

    public GameObject[] NewTraps;  // Array de NewTraps que la c�mara seguir�
    private int CurrentTrap = 0;

    public bool TransitionOn;

    public Animator Transitionanimator;  // Asigna esto desde el Inspector
    public string animationName; // Nombre de la animaci�n o trigger
    public GameObject Player;
    public GameObject CanvasHud;
    public MoveCamera moveCameraScript;
    public Camera mainCamera;
    public Camera transitionCamera;

    public GameObject[] ThingsToDesactivate;
    public GameObject[] AssistThingsToDesactivate;
    public GameObject[] ThingsToActivate;  // Nueva lista para objetos a activar
    public GameObject[] AssistThingsToActivate;  // Nueva lista para objetos a activar ASISTIDOS

    //
    [SerializeField] private PlayerMovementGrappling playerMovement;
    [SerializeField] private PlayerCam camMovement;
    //[SerializeField] private Grappling grappling;
    //


    void Start()
    {
        // Aseg�rate de que la animaci�n no se reproduzca al inicio
        Transitionanimator.enabled = false;


        //
        if (saveGameManager == null)
        {
            saveGameManager = FindObjectOfType<JsonSaveGameManager>();
        }

        if (saveGameManager == null)
        {
            Debug.LogError("JsonSaveGameManager no encontrado en la escena.");
        }

        Debug.Log("Estado de easyMode: " + saveGameManager.saveData.easyMode);
        //
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.K)) // Puedes cambiar Space por otra tecla o condici�n
        {
            StartCoroutine(x());
            PlayAnimation();
        }*/
    }

    public void callCinematic()
    {
        StartCoroutine(x());
    }

    public IEnumerator x()
    {
        //Player.gameObject.SetActive(false);

        //
        playerMovement.enabled = false;
        camMovement.enabled = false;
        //grappling.enabled = false;
        //

        CanvasHud.gameObject.SetActive(false);

        foreach (GameObject obj in ThingsToDesactivate)
        {
            obj.SetActive(false);
        }

        // ASSIST DESACTIVATED
        foreach (GameObject objAssist in AssistThingsToDesactivate)
        {
            if (saveGameManager.saveData.easyMode)
            {
                objAssist.SetActive(false);
                //Debug.Log(objAssist.name + " activado.");
            }
        }
        //

        moveCameraScript.enabled = false;
        mainCamera.enabled = false;
        transitionCamera.gameObject.SetActive(true);
        transitionCamera.enabled = true;

        Transitionanimator.enabled = true;  // Activa el Animator
        Transitionanimator.Play(animationName);  // Reproduce la animaci�n
        yield return null;

        ActivateThings();  // Llama al m�todo para activar objetos
    }

    public void PlayAnimation()
    {
        //Player.gameObject.SetActive(false);
        //
        playerMovement.enabled = false;
        camMovement.enabled = false;
        //grappling.enabled = false;
        //
        CanvasHud.gameObject.SetActive(false);

        foreach (GameObject obj in ThingsToDesactivate)
        {
            obj.SetActive(false);
        }

        // ASSIST DESACTIVATED
        foreach (GameObject objAssist in AssistThingsToDesactivate)
        {
            if (saveGameManager.saveData.easyMode)
            {
                objAssist.SetActive(false);
                //Debug.Log(objAssist.name + " activado.");
            }
        }
        //

        moveCameraScript.enabled = false;
        mainCamera.enabled = false;
        transitionCamera.enabled = true;

        Transitionanimator.enabled = true;  // Activa el Animator
        Transitionanimator.Play(animationName);  // Reproduce la animaci�n

        ActivateThings();  // Llama al m�todo para activar objetos
    }

    public void TransitionFunction()
    {
        NewTraps[CurrentTrap].gameObject.SetActive(true);
        CurrentTrap++;
        Debug.Log("xd");
    }

    private void ActivateThings()  // Nuevo m�todo para activar objetos
    {
        ActivateAssistThings();
        foreach (GameObject obj in ThingsToActivate)
        {
            obj.SetActive(true);  // Activa cada objeto en la lista
        }
    }

    private void ActivateAssistThings()
    {
        if (saveGameManager == null || saveGameManager.saveData == null)
        {
            Debug.LogError("saveGameManager o saveData no est�n configurados.");
            return;
        }

        Debug.Log("EasyMode est� en: " + saveGameManager.saveData.easyMode);

        foreach (GameObject objAssist in AssistThingsToActivate)
        {
            if (saveGameManager.saveData.easyMode)
            {
                objAssist.SetActive(true);
                Debug.Log(objAssist.name + " activado.");
            }
            else
            {
                Debug.Log(objAssist.name + " no se activ� porque EasyMode est� desactivado.");
            }
        }
    }

}
