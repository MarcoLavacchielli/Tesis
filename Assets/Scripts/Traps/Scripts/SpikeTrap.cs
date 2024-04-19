using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public Transform spikeTransform;
    public float moveDistance = 1f;
    public float moveSpeed = 1f;

    private bool playerInside = false;
    private Vector3 initialPosition;
    private Vector3 targetPosition;

    private void Start()
    {
        initialPosition = spikeTransform.position;
        targetPosition = initialPosition + Vector3.up * moveDistance;
    }

    private void Update()
    {
        if (playerInside)
        {
            spikeTransform.position = Vector3.MoveTowards(spikeTransform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            spikeTransform.position = Vector3.MoveTowards(spikeTransform.position, initialPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}