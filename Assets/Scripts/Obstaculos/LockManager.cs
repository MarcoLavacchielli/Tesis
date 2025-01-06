using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public LockColumn[] columnas; // Array de columnas
    private int columnaActual = 0; // Índice de la columna activa
    public float interactionRadius = 5f;   // Radio de interacción
    public LayerMask playerLayerMask;      // LayerMask para el jugador

    void Start()
    {
        ActivarColumnaActual();
    }

    void Update()
    {
        // Comprobar si el jugador está en el rango y presiona Enter
        if (IsPlayerInRange() && Input.GetKeyDown(KeyCode.Return))
        {
            ProcesarColumnaActual();
        }
    }

    private void ProcesarColumnaActual()
    {
        if (columnaActual >= 0 && columnaActual < columnas.Length)
        {
            if (columnas[columnaActual].TryStopColumn())
            {
                // Columna resuelta correctamente
                Debug.Log("Código correcto. Avanzando a la siguiente columna.");
                columnaActual++;
                if (columnaActual >= columnas.Length)
                {
                    // Puzzle completado
                    Debug.Log("¡Puzzle resuelto! Desbloqueando...");
                }
                else
                {
                    ActivarColumnaActual();
                }
            }
            else
            {
                // Código incorrecto, retroceder una columna
                Debug.Log("Código incorrecto. Retrocediendo a la columna anterior.");
                if (columnaActual > 0)
                {
                    columnaActual--;
                }
                ActivarColumnaActual();
            }
        }
    }

    private void ActivarColumnaActual()
    {
        for (int i = 0; i < columnas.Length; i++)
        {
            if (i == columnaActual)
            {
                columnas[i].ResetColumn();
            }
        }
    }

    // Función que verifica si el jugador está dentro del radio de interacción
    private bool IsPlayerInRange()
    {
        // Buscar al jugador en la escena por su capa
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return false;

        // Verificar si el jugador está en la capa correcta usando LayerMask
        if ((playerLayerMask.value & (1 << playerObject.layer)) != 0)
        {
            float distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
            return distanceToPlayer <= interactionRadius;
        }
        return false;
    }

    // Dibuja el Gizmo en la escena para visualizar el radio de interacción
    private void OnDrawGizmos()
    {
        // Configura el color del Gizmo
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Verde semitransparente
        // Dibuja la esfera de interacción
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
