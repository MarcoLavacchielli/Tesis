using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTeleporter : MonoBehaviour
{
    [SerializeField] private List<Vector3> waypoints; // Lista de posiciones de waypoints
    [SerializeField] private PlayerMovementGrappling player;
    [SerializeField] private Rigidbody playerRb;

    // Asigna el script de Grappling aqu�
    [SerializeField] private Grappling grapplingScript;

    public int currentWaypointIndex = 0; // �ndice del waypoint actual

    private void Awake()
    {
        currentWaypointIndex = 0;
    }

    // Llamar cuando el jugador alcance un nuevo waypoint para actualizar el �ndice
    public void ReachNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count - 1)
        {
            currentWaypointIndex++;
            Debug.Log("Nuevo waypoint alcanzado: " + waypoints[currentWaypointIndex]);
        }
    }

    public void Activate()
    {
        Debug.Log("Spike trap activated!");

        // Detener el grappling si est� activo
        grapplingScript.InterruptGrapple();

        // Asegurarse de tener el Rigidbody del jugador
        if (playerRb != null && currentWaypointIndex < waypoints.Count)
        {
            // Hacer el Rigidbody cinem�tico para evitar movimiento
            playerRb.isKinematic = true;

            // Teletransportar al jugador al waypoint activo
            player.transform.position = waypoints[waypoints.Count - 1];

            Debug.Log("Jugador teletransportado al �ltimo waypoint: " + waypoints[waypoints.Count - 1]);

            // Restaurar el Rigidbody a no cinem�tico despu�s de la teletransportaci�n
            StartCoroutine(ResetRigidbody());
        }
    }

    public void Death()
    {
        Debug.Log("Spike trap activated!");

        // Detener el grappling si est� activo
        grapplingScript.InterruptGrapple();

        // Asegurarse de tener el Rigidbody del jugador
        if (playerRb != null && waypoints.Count > 0)
        {
            // Hacer el Rigidbody cinem�tico para evitar movimiento
            playerRb.isKinematic = true;

            // Teletransportar al jugador al punto de muerte (primer waypoint)
            player.transform.position = waypoints[currentWaypointIndex];

            Debug.Log("Jugador teletransportado al punto de origen: " + waypoints[0]);

            // Restaurar el Rigidbody a no cinem�tico despu�s de la teletransportaci�n
            StartCoroutine(ResetRigidbody());
        }
    }

    private IEnumerator ResetRigidbody()
    {
        // Opcionalmente, esperar un frame para asegurar que la teletransportaci�n ocurra
        yield return null;

        // Restaurar el Rigidbody a comportamiento normal (no cinem�tico)
        playerRb.isKinematic = false;

        // Asegurarse de que la velocidad del jugador sea cero despu�s de la teletransportaci�n
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        Debug.Log("Movimiento del jugador habilitado despu�s de la teletransportaci�n.");
    }
}
