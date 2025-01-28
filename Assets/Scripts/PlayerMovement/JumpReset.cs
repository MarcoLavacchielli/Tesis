using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpReset : MonoBehaviour
{

    public Material apagado;

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entró en el trigger es el jugador
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Obtener el componente PlayerMovementGrappling del jugador
            PlayerMovementGrappling playerMovement = other.GetComponent<PlayerMovementGrappling>();

            if (playerMovement != null)
            {
                // rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
                playerMovement.rb.AddForce(transform.up * playerMovement.jumpForce, ForceMode.Impulse);
                Destroy(gameObject);  //OPCIONAL -> Destruye el objeto a tocarlo
            }
        }

    }
}
