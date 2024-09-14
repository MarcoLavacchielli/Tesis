using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTeleporter : MonoBehaviour
{

    [SerializeField] private Vector3 checkPoint;
    private PlayerMovementGrappling player;
    private Rigidbody playerRb;

    public void Activate()
    {
        Debug.Log("Spike trap activated!");

        // Si el diamante ha sido tomado, teletransportamos al jugador al checkpoint
        if (playerRb != null)
        {
            playerRb.velocity = Vector3.zero;  // Detener el movimiento del jugador
            playerRb.angularVelocity = Vector3.zero;  // Detener la rotación del jugador

            // Teletransportar al checkpoint
            player.transform.position = checkPoint;

            Debug.Log("Jugador teletransportado a la posición del checkpoint: " + checkPoint);
        }
    }
}
