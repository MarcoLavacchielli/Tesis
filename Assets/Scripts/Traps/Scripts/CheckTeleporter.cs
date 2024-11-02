using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTeleporter : MonoBehaviour
{
    [SerializeField] private List<Vector3> waypoints; // Lista de posiciones de waypoints
    [SerializeField] private PlayerMovementGrappling player;
    [SerializeField] private Rigidbody playerRb;

    // Asigna el script de Grappling aquí
    [SerializeField] private Grappling grapplingScript;

    public int currentWaypointIndex = 0; // Índice del waypoint actual

    private void Awake()
    {
        currentWaypointIndex = 0;
    }

    // Llamar cuando el jugador alcance un nuevo waypoint para actualizar el índice
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

        // Detener el grappling si está activo
        grapplingScript.InterruptGrapple();

        // Asegurarse de tener el Rigidbody del jugador
        if (playerRb != null && currentWaypointIndex < waypoints.Count)
        {
            // Hacer el Rigidbody cinemático para evitar movimiento
            playerRb.isKinematic = true;

            // Teletransportar al jugador al waypoint activo
            player.transform.position = waypoints[waypoints.Count - 1];

            Debug.Log("Jugador teletransportado al último waypoint: " + waypoints[waypoints.Count - 1]);

            // Restaurar el Rigidbody a no cinemático después de la teletransportación
            StartCoroutine(ResetRigidbody());
        }
    }

    public void Death()
    {
        Debug.Log("Spike trap activated!");

        // Detener el grappling si está activo
        grapplingScript.InterruptGrapple();

        // Asegurarse de tener el Rigidbody del jugador
        if (playerRb != null && waypoints.Count > 0)
        {
            // Hacer el Rigidbody cinemático para evitar movimiento
            playerRb.isKinematic = true;

            // Teletransportar al jugador al punto de muerte (primer waypoint)
            player.transform.position = waypoints[currentWaypointIndex];

            Debug.Log("Jugador teletransportado al punto de origen: " + waypoints[0]);

            // Restaurar el Rigidbody a no cinemático después de la teletransportación
            StartCoroutine(ResetRigidbody());
        }
    }

    private IEnumerator ResetRigidbody()
    {
        // Opcionalmente, esperar un frame para asegurar que la teletransportación ocurra
        yield return null;

        // Restaurar el Rigidbody a comportamiento normal (no cinemático)
        playerRb.isKinematic = false;

        // Asegurarse de que la velocidad del jugador sea cero después de la teletransportación
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        Debug.Log("Movimiento del jugador habilitado después de la teletransportación.");
    }
}
