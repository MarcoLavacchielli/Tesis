using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransitionsEvents : MonoBehaviour
{
    public GameObject[] NewTraps;  // Array de NewTraps que la cámara seguirá

    private int CurrentTrap = 0;

    public bool TransitionOn;

    public Animator Transitionanimator;  // Asigna esto desde el Inspector
    public string animationName; // Nombre de la animación o trigger
    public GameObject Player;
    public MoveCamera moveCameraScript;
    public Camera mainCamera;
    public Camera transitionCamera;


    public void TransitionFunction()
    {
        NewTraps[CurrentTrap].gameObject.SetActive(true);
        CurrentTrap++;
        Debug.Log("xd");
    }

    public void EndCinematicFunc()
    {
        Transitionanimator.enabled = false; 
        transitionCamera.enabled = false;
        transitionCamera.gameObject.SetActive(false);
        
        moveCameraScript.enabled = true;
        Player.gameObject.SetActive(true);
        mainCamera.enabled = true;

       
    }
}
