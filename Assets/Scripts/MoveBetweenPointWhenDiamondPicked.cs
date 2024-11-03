using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPointWhenDiamondPicked : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    public float speed = 1.0f;

    private float startTime;
    private float journeyLength;
    private bool isMovingToEnd = true; // Indica si el objeto se está moviendo hacia el punto final

    public TakeDiamont diamond;

    void Start()
    {
        startTime = Time.time;
        journeyLength = Vector3.Distance(startPoint.position, endPoint.position);
    }

    void Update()
    {
        if (diamond.diamondTake == true)
        {

            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            if (isMovingToEnd)
            {
                transform.position = Vector3.Lerp(startPoint.position, endPoint.position, fractionOfJourney);
            }
            else
            {
                transform.position = Vector3.Lerp(endPoint.position, startPoint.position, fractionOfJourney);
            }

            if (fractionOfJourney >= 1.0f)
            {
                isMovingToEnd = !isMovingToEnd; // Cambiar la dirección del movimiento
                startTime = Time.time;
            }

        }
    }
}
