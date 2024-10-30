using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondChangeMaterial : MonoBehaviour
{
    // Listas de GameObjects que vas a modificar
    public GameObject[] objetos;
    public GameObject[] objetosVariante;
    public GameObject[] objetosTileados;
    public GameObject[] objetosLed;

    // Lista de partículas
    public GameObject[] particulas;

    // Materiales que se asignarán a los objetos
    public Material nuevoMaterial;
    public Material nuevoMaterialVariante;
    public Material nuevoMaterialTileados;
    public Material nuevoMaterialLed;
    public Material nuevoMaterialParticulas;

    // Función que cambia el material de todos los objetos en la lista
    public void CambiarMaterial()
    {
        foreach (GameObject objeto in objetos)
        {
            Renderer renderer = objeto.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = nuevoMaterial;
            }
        }

        foreach (GameObject objetoVariante in objetosVariante)
        {
            Renderer renderer2 = objetoVariante.GetComponent<Renderer>();
            if (renderer2 != null)
            {
                renderer2.material = nuevoMaterialVariante;
            }
        }

        foreach (GameObject objetoTileado in objetosTileados)
        {
            Renderer renderer3 = objetoTileado.GetComponent<Renderer>();
            if (renderer3 != null)
            {
                renderer3.material = nuevoMaterialTileados;
            }
        }

        foreach (GameObject objetoLed in objetosLed)
        {
            Renderer renderer4 = objetoLed.GetComponent<Renderer>();
            if (renderer4 != null)
            {
                renderer4.material = nuevoMaterialLed;
            }
        }

        // Cambiar el material de las partículas
        foreach (GameObject particula in particulas)
        {
            Renderer rendererParticula = particula.GetComponent<Renderer>();
            if (rendererParticula != null)
            {
                rendererParticula.material = nuevoMaterialParticulas;
            }
        }
    }
}
