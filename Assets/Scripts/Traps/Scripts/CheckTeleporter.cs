using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTeleporter : MonoBehaviour
{
    [SerializeField] private Vector3 checkPoint;
    [SerializeField] private Vector3 deathPoint;
    [SerializeField] private PlayerMovementGrappling player;
    [SerializeField] private Rigidbody playerRb;

    // Asigna el script de Grappling aquí
    [SerializeField] private Grappling grapplingScript;

    public void Activate()
    {
        Debug.Log("Spike trap activated!");

        // Detener el grappling si está activo
        grapplingScript.InterruptGrapple();

        // Ensure we have the player's Rigidbody
        if (playerRb != null)
        {
            // Make the Rigidbody kinematic to prevent further movement
            playerRb.isKinematic = true;

            // Immediately set the player's position to the checkpoint
            player.transform.position = checkPoint;

            Debug.Log("Player teleported to the checkpoint: " + checkPoint);

            // Restore the Rigidbody to non-kinematic after the teleportation
            StartCoroutine(ResetRigidbody());
        }
    }

    public void Death()
    {
        Debug.Log("Spike trap activated!");

        // Detener el grappling si está activo
        grapplingScript.InterruptGrapple();

        // Ensure we have the player's Rigidbody
        if (playerRb != null)
        {
            // Make the Rigidbody kinematic to prevent further movement
            playerRb.isKinematic = true;

            // Immediately set the player's deathPoint to the checkpoint
            player.transform.position = deathPoint;

            Debug.Log("Player teleported to the death point: " + deathPoint);

            // Restore the Rigidbody to non-kinematic after the teleportation
            StartCoroutine(ResetRigidbody());
        }
    }

    private IEnumerator ResetRigidbody()
    {
        // Optionally, wait a frame to ensure the teleportation happens
        yield return null;

        // Restore Rigidbody to normal (non-kinematic) behavior
        playerRb.isKinematic = false;

        // Ensure the player's velocity is zero after the teleport
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        Debug.Log("Player movement re-enabled after teleportation.");
    }
}
