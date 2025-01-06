using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public LockColumn[] columnas; // Array de columnas
    private int columnaActual = 0; // �ndice de la columna activa
    public float interactionRadius = 5f;   // Radio de interacci�n
    public LayerMask playerLayerMask;      // LayerMask para el jugador

    void Start()
    {
        ActivarColumnaActual();
    }

    void Update()
    {
        // Comprobar si el jugador est� en el rango y presiona Enter
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
                Debug.Log("C�digo correcto. Avanzando a la siguiente columna.");
                columnaActual++;
                if (columnaActual >= columnas.Length)
                {
                    // Puzzle completado
                    Debug.Log("�Puzzle resuelto! Desbloqueando...");
                }
                else
                {
                    ActivarColumnaActual();
                }
            }
            else
            {
                // C�digo incorrecto, retroceder una columna
                Debug.Log("C�digo incorrecto. Retrocediendo a la columna anterior.");
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

    // Funci�n que verifica si el jugador est� dentro del radio de interacci�n
    private bool IsPlayerInRange()
    {
        // Buscar al jugador en la escena por su capa
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return false;

        // Verificar si el jugador est� en la capa correcta usando LayerMask
        if ((playerLayerMask.value & (1 << playerObject.layer)) != 0)
        {
            float distanceToPlayer = Vector3.Distance(playerObject.transform.position, transform.position);
            return distanceToPlayer <= interactionRadius;
        }
        return false;
    }

    // Dibuja el Gizmo en la escena para visualizar el radio de interacci�n
    private void OnDrawGizmos()
    {
        // Configura el color del Gizmo
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Verde semitransparente
        // Dibuja la esfera de interacci�n
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
