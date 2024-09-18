using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedesTron : MonoBehaviour
{
    // Listas para los diferentes bordes
    public List<GameObject> bordeAbajoDerecha;
    public List<GameObject> bordeAbajoIzquierda;
    public List<GameObject> bordeAbajoMedia;

    public List<GameObject> bordeArribaDerecha;
    public List<GameObject> bordeArribaIzquierda;
    public List<GameObject> bordeArribaMedia;

    public List<GameObject> bordeMedioDerecha;
    public List<GameObject> bordeMedioIzquierda;
    public List<GameObject> bordeMedioMedia;

    void Start()
    {
        // Selecciona uno de los elementos aleatoriamente y desactiva el resto
        ActivateRandomElement(bordeAbajoDerecha);
        ActivateRandomElement(bordeAbajoIzquierda);
        ActivateRandomElement(bordeAbajoMedia);

        ActivateRandomElement(bordeArribaDerecha);
        ActivateRandomElement(bordeArribaIzquierda);
        ActivateRandomElement(bordeArribaMedia);

        ActivateRandomElement(bordeMedioDerecha);
        ActivateRandomElement(bordeMedioIzquierda);
        ActivateRandomElement(bordeMedioMedia);
    }

    // Función para activar un elemento aleatorio y desactivar el resto
    void ActivateRandomElement(List<GameObject> list)
    {
        if (list.Count == 0) return;

        int randomIndex = Random.Range(0, list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SetActive(i == randomIndex);
        }
    }
}
