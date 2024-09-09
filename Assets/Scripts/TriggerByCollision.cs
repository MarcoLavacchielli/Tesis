using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerByCollision : MonoBehaviour
{
    #region Funcionalidad

    // Referencias públicas a los objetos pisoDerecho y pisoIzquierdo
    public GameObject pisoDerecho;
    public GameObject pisoIzquierdo;

    // Desplazamiento para el movimiento en el eje Z
    public float desplazamiento = 5.0f;

    // Tiempo que durará la animación
    public float duracionAnimacion = 1.0f;

    // Booleano para controlar que la animación se ejecute solo una vez
    private bool animacionEjecutada = false;

    // Listas públicas para plataformas y sus nuevas posiciones
    public List<GameObject> plataformas;
    public List<Transform> posicionesPlatNuevas;

    // Método que se ejecuta cuando otro objeto colisiona con este
    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto que colisiona tiene la capa "Player" y que la animación no se haya ejecutado antes
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !animacionEjecutada)
        {
            // Iniciar la animación de movimiento de ambos objetos
            StartCoroutine(MoverPisosAnimadamente());

            // Mover plataformas a nuevas posiciones
            StartCoroutine(MoverPlataformasAnimadamente());

            // Marcar la animación como ejecutada para que no se repita
            animacionEjecutada = true;
        }
    }

    // Corutina que realiza el movimiento suave de los pisos
    IEnumerator MoverPisosAnimadamente()
    {
        Vector3 posicionInicialDerecho = pisoDerecho.transform.position;
        Vector3 posicionInicialIzquierdo = pisoIzquierdo.transform.position;

        Vector3 posicionFinalDerecho = posicionInicialDerecho + new Vector3(desplazamiento, 0, 0);
        Vector3 posicionFinalIzquierdo = posicionInicialIzquierdo + new Vector3(-desplazamiento, 0, 0);

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracionAnimacion)
        {
            // Calcular la interpolación lineal (lerp) entre la posición inicial y final
            pisoDerecho.transform.position = Vector3.Lerp(posicionInicialDerecho, posicionFinalDerecho, tiempoTranscurrido / duracionAnimacion);
            pisoIzquierdo.transform.position = Vector3.Lerp(posicionInicialIzquierdo, posicionFinalIzquierdo, tiempoTranscurrido / duracionAnimacion);

            tiempoTranscurrido += Time.deltaTime;

            yield return null; // Esperar un frame antes de continuar
        }

        // Asegurarse de que la posición final sea la correcta
        pisoDerecho.transform.position = posicionFinalDerecho;
        pisoIzquierdo.transform.position = posicionFinalIzquierdo;
    }

    // Corutina que realiza el movimiento suave de las plataformas
    IEnumerator MoverPlataformasAnimadamente()
    {
        // Comprobar que las listas de plataformas y posiciones tengan la misma cantidad de elementos
        if (plataformas.Count != posicionesPlatNuevas.Count)
        {
            Debug.LogError("Las listas de plataformas y posiciones deben tener la misma cantidad de elementos.");
            yield break;
        }

        for (int i = 0; i < plataformas.Count; i++)
        {
            GameObject plataforma = plataformas[i];
            Transform nuevaPosicion = posicionesPlatNuevas[i];

            if (plataforma != null && nuevaPosicion != null)
            {
                Vector3 posicionInicial = plataforma.transform.position;
                Vector3 posicionFinal = nuevaPosicion.position;

                float tiempoTranscurrido = 0f;

                while (tiempoTranscurrido < duracionAnimacion)
                {
                    // Mover la plataforma hacia su nueva posición de forma suave
                    plataforma.transform.position = Vector3.Lerp(posicionInicial, posicionFinal, tiempoTranscurrido / duracionAnimacion);

                    tiempoTranscurrido += Time.deltaTime;
                    yield return null; // Esperar un frame antes de continuar
                }

                // Asegurarse de que la posición final sea la correcta
                plataforma.transform.position = posicionFinal;
            }
        }
    }

    #endregion
}
