using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenMultiplePoints : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints; // Lista de puntos para patrullar
    public float speed = 1.0f;

    private int currentPointIndex = 0; // Índice del punto actual en el que se encuentra el objeto
    private float startTime;
    private float journeyLength;

    void Start()
    {
        if (patrolPoints.Length < 2)
        {
            Debug.LogError("Necesitas al menos 2 puntos de patrulla.");
            enabled = false;
            return;
        }

        SetNextDestination();
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        transform.position = Vector3.Lerp(patrolPoints[currentPointIndex].position, patrolPoints[GetNextPointIndex()].position, fractionOfJourney);

        if (fractionOfJourney >= 1.0f)
        {
            currentPointIndex = GetNextPointIndex();
            SetNextDestination();
        }
    }

    private void SetNextDestination()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(patrolPoints[currentPointIndex].position, patrolPoints[GetNextPointIndex()].position);
    }

    private int GetNextPointIndex()
    {
        return (currentPointIndex + 1) % patrolPoints.Length; // Ciclo a través de los puntos
    }
}
