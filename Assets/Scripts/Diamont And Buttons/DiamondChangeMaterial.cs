using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondChangeMaterial : MonoBehaviour
{
    // Lista de GameObjects que vas a modificar
    public GameObject[] objetos;

    // Material que se asignará a los objetos
    public Material nuevoMaterial;

    void Update()
    {
        // Detecta si la tecla 'N' ha sido presionada
        if (Input.GetKeyDown(KeyCode.N))
        {
            CambiarMaterial();
        }
    }

    // Función que cambia el material de todos los objetos en la lista
    void CambiarMaterial()
    {
        foreach (GameObject objeto in objetos)
        {
            // Verifica si el GameObject tiene un Renderer
            Renderer renderer = objeto.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = nuevoMaterial;
            }
        }
    }
}
