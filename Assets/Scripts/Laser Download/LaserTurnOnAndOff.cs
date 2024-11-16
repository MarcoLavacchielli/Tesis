using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurnOnAndOff : MonoBehaviour
{
    public List<GameObject> objetosALuminar; // Lista de objetos a iluminar
    public List<ParticleSystem> particulas; // Lista de sistemas de partículas
    public float tiempoApagado = 2.0f;      // Tiempo que los objetos estarán apagados
    public float tiempoEncendido = 3.0f;    // Tiempo que los objetos estarán encendidos
    private float tiempoAntesEncendido = 1f; // Tiempo antes de encender para activar las partículas

    private void Start()
    {
        StartCoroutine(AlternarEncendidoApagado());
    }

    private IEnumerator AlternarEncendidoApagado()
    {
        while (true)
        {
            // Apagar los objetos
            foreach (GameObject objeto in objetosALuminar)
            {
                if (objeto != null) objeto.SetActive(false);
            }

            yield return new WaitForSeconds(tiempoApagado - tiempoAntesEncendido);

            // Reproducir las partículas
            foreach (ParticleSystem particula in particulas)
            {
                if (particula != null && !particula.isPlaying) particula.Play();
            }

            yield return new WaitForSeconds(tiempoAntesEncendido);

            // Encender los objetos y detener las partículas
            foreach (GameObject objeto in objetosALuminar)
            {
                if (objeto != null) objeto.SetActive(true);
            }

            foreach (ParticleSystem particula in particulas)
            {
                if (particula != null && particula.isPlaying) particula.Stop();
            }

            yield return new WaitForSeconds(tiempoEncendido);
        }
    }
}
