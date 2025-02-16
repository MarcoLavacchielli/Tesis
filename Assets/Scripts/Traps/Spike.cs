using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spike : Trap
{
    public int damageAmount;
    public Vector3 checkPoint;
    public Vector3 deathPoint;

    [SerializeField] private TakeDiamont diamond;
    private PlayerMovementGrappling player;
    private Rigidbody playerRb;

    public CheckTeleporter check;

    /*private void Awake()
    {
        diamond = GetComponent<TakeDiamont>(); // Asumiendo que el diamante est� en el mismo objeto que la trampa
    }*/

    private void OnCollisionEnter(Collision other)
    {
        // Si el jugador colisiona con la trampa
        if (other.gameObject.CompareTag("Player")) // Aseg�rate de que el jugador tenga el tag "Player"
        {
            player = other.gameObject.GetComponent<PlayerMovementGrappling>(); // Obtenemos el script de movimiento del jugador
            playerRb = other.gameObject.GetComponent<Rigidbody>(); // Obtenemos el Rigidbody del jugador

            Activate();
        }
    }

    public override void Activate()
    {
        Debug.Log("Spike trap activated!");

        // Si el diamante no ha sido tomado, reinicia la escena
        if (diamond.diamondTake == false || diamond.diamondTake == true)
        {
            /*
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Si el diamante ha sido tomado, teletransportamos al jugador al checkpoint
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;  // Detener el movimiento del jugador
                playerRb.angularVelocity = Vector3.zero;  // Detener la rotaci�n del jugador

                // Teletransportar al checkpoint
                player.transform.position = deathPoint;

                Debug.Log("Jugador teletransportado a la posici�n del checkpoint: " + deathPoint);
            }
            */
            check.Death();
        }
        else
        {
            /*
            // Si el diamante ha sido tomado, teletransportamos al jugador al checkpoint
            if (playerRb != null)
            {
                playerRb.velocity = Vector3.zero;  // Detener el movimiento del jugador
                playerRb.angularVelocity = Vector3.zero;  // Detener la rotaci�n del jugador

                // Teletransportar al checkpoint
                player.transform.position = checkPoint;

                Debug.Log("Jugador teletransportado a la posici�n del checkpoint: " + checkPoint);
            }
            */
            check.Death();
        }
    }
}
