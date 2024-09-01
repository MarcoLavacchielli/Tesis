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

    private IEnumerator MultipleLaserAnimation(GameObject pieza)
    {
        Vector3 destino = pieza.transform.position + new Vector3(0, 1.7f, 0);

        float duracion = 1.5f; // Duración de la animación
        float velocidad = 7f / duracion; // Velocidad constante

        while (Vector3.Distance(pieza.transform.position, destino) > 0.01f)
        {
            pieza.transform.position = Vector3.MoveTowards(pieza.transform.position, destino, velocidad * Time.deltaTime);
            yield return null;

        }

        NewTraps[4].gameObject.SetActive(true);
        NewTraps[5].gameObject.SetActive(true);
        NewTraps[6].gameObject.SetActive(true);
        NewTraps[7].gameObject.SetActive(true);
        pieza.transform.position = destino;
        CurrentTrap = 8;
    }
    public void TransitionFunction()
    {
        if (CurrentTrap == 4)
        {
            StartCoroutine(MultipleLaserAnimation(NewTraps[CurrentTrap]));
            Debug.Log("animacion");

        }
        else
        {
            NewTraps[CurrentTrap].gameObject.SetActive(true);
            CurrentTrap++;
            Debug.Log("xd");

        }
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
