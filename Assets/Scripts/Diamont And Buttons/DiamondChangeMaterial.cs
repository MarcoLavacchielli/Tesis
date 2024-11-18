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
    public GameObject[] objetosAnimados; // Nueva lista de objetos animados
    public GameObject[] objetosParedesWallrun; // Nueva lista de objetos para paredes de wallrun
    public GameObject[] objetosRieles; // Nueva lista de objetos para rieles
    public GameObject[] objetosVitrinasFacu; // Nueva lista de objetos para vitrinas facu

    // Lista de partículas
    public GameObject[] particulas;

    // Materiales que se asignarán a los objetos
    public Material nuevoMaterial;
    public Material nuevoMaterialVariante;
    public Material nuevoMaterialTileados;
    public Material nuevoMaterialLed;
    public Material nuevoMaterialAnimado; // Nuevo material para los objetos animados
    public Material nuevoMaterialParedesWallrun; // Nuevo material para las paredes de wallrun
    public Material nuevoMaterialParticulas;
    public Material nuevoMaterialRiel; // Nuevo material para los rieles
    public Material nuevoMaterialVitrinasFacu; // Nuevo material para las vitrinas facu

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

        // Cambiar el material de los objetos animados
        foreach (GameObject objetoAnimado in objetosAnimados)
        {
            Renderer renderer5 = objetoAnimado.GetComponent<Renderer>();
            if (renderer5 != null)
            {
                renderer5.material = nuevoMaterialAnimado;
            }
        }

        // Cambiar el material de las paredes de wallrun
        foreach (GameObject objetoParedWallrun in objetosParedesWallrun)
        {
            Renderer renderer6 = objetoParedWallrun.GetComponent<Renderer>();
            if (renderer6 != null)
            {
                renderer6.material = nuevoMaterialParedesWallrun;
            }
        }

        // Cambiar el material de los rieles
        foreach (GameObject objetoRiel in objetosRieles)
        {
            Renderer rendererRiel = objetoRiel.GetComponent<Renderer>();
            if (rendererRiel != null)
            {
                rendererRiel.material = nuevoMaterialRiel;
            }
        }

        // Cambiar el material de las vitrinas facu
        foreach (GameObject objetoVitrinaFacu in objetosVitrinasFacu)
        {
            Renderer rendererVitrinaFacu = objetoVitrinaFacu.GetComponent<Renderer>();
            if (rendererVitrinaFacu != null)
            {
                rendererVitrinaFacu.material = nuevoMaterialVitrinasFacu;
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
