using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public Transform[] waypoints;  // Array de waypoints que la cámara seguirá
    public float speed = 5f;       // Velocidad de movimiento de la cámara
    public float rotationSpeed = 2f;  // Velocidad de rotación suave de la cámara

    private int currentWaypointIndex = 0;

    public bool TransitionOn;
    public MoveCamera movecameraScript;

    void Update()
    {
        if (TransitionOn==true)
        {
            movecameraScript.enabled = false;
            /*
            // Si no hay waypoints, no hacemos nada
            if (waypoints.Length == 0) return;

            // Obtener el waypoint actual
            Transform targetWaypoint = waypoints[currentWaypointIndex];

            // Mover la cámara hacia el waypoint actual
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            // Rotar suavemente la cámara hacia el siguiente waypoint
            Vector3 directionToTarget = targetWaypoint.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Verificar si la cámara ha llegado al waypoint actual
            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                // Mover al siguiente waypoint si está disponible
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }*/



        }
    }
}
