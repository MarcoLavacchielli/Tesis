using UnityEngine;

public class SpeedDownTrap : MonoBehaviour
{
    public bool playerIsInside = false;
    public PlayerMovementAdvanced playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.walkSpeed /= 2f;
            playerMovement.sprintSpeed /= 2f;
            playerMovement.slideSpeed /= 2f;
            playerMovement.wallrunSpeed /= 2f;
            playerMovement.climbSpeed /= 2f;
            playerMovement.groundDrag /= 20f;
            playerIsInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement.walkSpeed *= 2f;
            playerMovement.sprintSpeed *= 2f;
            playerMovement.slideSpeed *= 2f;
            playerMovement.wallrunSpeed *= 2f;
            playerMovement.climbSpeed *= 2f;
            playerMovement.groundDrag *= 20f;
            playerIsInside = false;
        }
    }
}