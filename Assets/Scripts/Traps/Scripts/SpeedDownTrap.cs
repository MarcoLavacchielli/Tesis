using UnityEngine;

public class SpeedDownTrap : MonoBehaviour
{
    public LayerMask playerLayer;
    public bool isIn = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            PlayerMovementAdvanced playerMovement = collision.gameObject.GetComponent<PlayerMovementAdvanced>();

            if (playerMovement != null)
            {
                playerMovement.walkSpeed /= 2f;
                playerMovement.sprintSpeed /= 2f;
                playerMovement.slideSpeed /= 2f;
                playerMovement.wallrunSpeed /= 2f;
                playerMovement.climbSpeed /= 2f;
                playerMovement.groundDrag /= 20f;
                isIn = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        PlayerMovementAdvanced playerMovement = collision.gameObject.GetComponent<PlayerMovementAdvanced>();

        if (playerMovement != null)
        {
            playerMovement.walkSpeed *= 2f;
            playerMovement.sprintSpeed *= 2f;
            playerMovement.slideSpeed *= 2f;
            playerMovement.wallrunSpeed *= 2f;
            playerMovement.climbSpeed *= 2f;
            playerMovement.groundDrag *= 20f;
            isIn = false;
        }
    }
}