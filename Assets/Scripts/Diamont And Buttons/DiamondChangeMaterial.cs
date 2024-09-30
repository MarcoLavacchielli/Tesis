using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondChangeMaterial : MonoBehaviour
{
    // Lista de GameObjects que vas a modificar
    public GameObject[] objetos;

    public GameObject[] objetosVariante;

    // Material que se asignar� a los objetos
    public Material nuevoMaterial;

    public Material nuevoMaterialVariante;

    // Funci�n que cambia el material de todos los objetos en la lista
    public void CambiarMaterial()
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

        foreach (GameObject objetoVariante in objetosVariante)
        {
            // Verifica si el GameObject tiene un Renderer
            Renderer renderer2 = objetoVariante.GetComponent<Renderer>();
            if (renderer2 != null)
            {
                renderer2.material = nuevoMaterialVariante;
            }
        }
    }
}
