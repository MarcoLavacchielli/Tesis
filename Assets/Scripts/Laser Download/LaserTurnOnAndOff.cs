using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurnOnAndOff : MonoBehaviour
{
    public List<GameObject> objetosALuminar; 
    public float tiempoApagado = 2.0f;     
    public float tiempoEncendido = 3.0f;   

    private void Start()
    {
        StartCoroutine(AlternarEncendidoApagado());
    }

    private IEnumerator AlternarEncendidoApagado()
    {
        while (true)
        {
            foreach (GameObject objeto in objetosALuminar)
            {
                if (objeto != null) objeto.SetActive(false);
            }
            yield return new WaitForSeconds(tiempoApagado);

            foreach (GameObject objeto in objetosALuminar)
            {
                if (objeto != null) objeto.SetActive(true);
            }
            yield return new WaitForSeconds(tiempoEncendido);
        }
    }
}
