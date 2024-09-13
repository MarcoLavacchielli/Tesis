using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public GameObject[] NewTraps;  // Array de NewTraps que la cámara seguirá
  
    private int CurrentTrap = 0;

    public bool TransitionOn;
    
    public Animator Transitionanimator;  // Asigna esto desde el Inspector
    public string animationName; // Nombre de la animación o trigger
    public GameObject Player;
    public GameObject CanvasHud;
    public MoveCamera moveCameraScript;
    public Camera mainCamera;
    public Camera transitionCamera;

    public GameObject[] ThingsToDesactivate;

    void Start()
    {
        // Asegúrate de que la animación no se reproduzca al inicio
        Transitionanimator.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Puedes cambiar Space por otra tecla o condición
        {
            StartCoroutine(x());
            //PlayAnimation();
        }
    }

    public void callCinematic()
    {
        StartCoroutine(x());
    }

    public IEnumerator x()
    {

        Player.gameObject.SetActive(false);
        CanvasHud.gameObject.SetActive(false);

        foreach (GameObject obj in ThingsToDesactivate)
        {
            obj.SetActive(false);
        }

        moveCameraScript.enabled = false;
        //transform.position = new Vector3(197, 14, -137);
        //transform.rotation = new Quaternion(0f,0f,0f);
        mainCamera.enabled = false;
        transitionCamera.gameObject.SetActive(true);
        transitionCamera.enabled = true;

        //yield return new WaitForSeconds(2f);
        Transitionanimator.enabled = true;  // Activa el Animator
        Transitionanimator.Play(animationName);  // Reproduce la animación
        yield return null;
    }

    public void PlayAnimation()
    {
        Player.gameObject.SetActive(false);
        CanvasHud.gameObject.SetActive(false);

        foreach(GameObject obj in ThingsToDesactivate)
        {
            obj.SetActive(false);
        }

        moveCameraScript.enabled = false;
        transform.position = new Vector3(197, 14, -137);
        //transform.rotation = new Quaternion(0f,0f,0f);
        mainCamera.enabled = false;
        transitionCamera.enabled = true;
        
        Transitionanimator.enabled = true;  // Activa el Animator
        Transitionanimator.Play(animationName);  // Reproduce la animación
    }

    public void TransitionFunction()
    {
        NewTraps[CurrentTrap].gameObject.SetActive(true);
        CurrentTrap++;
        Debug.Log("xd");
    }
}
