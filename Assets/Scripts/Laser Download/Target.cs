using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Target : MonoBehaviour
{
    public TakeDiamont diamond;
    public CheckTeleporter check;
    [SerializeField] private Vector3 checkPoint;
    private Rigidbody rb;

    private void Start()
    {
        // Intentamos obtener el Rigidbody del objeto si tiene uno
        rb = GetComponent<Rigidbody>();
    }

    public void Hit()
    {
        Debug.Log("Target Hit " + name);

        if (diamond.diamondTake == false)
        {
            /*// Reiniciar la escena actual si no se ha tomado el diamante
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);*/
            check.Death();
        }
        else
        {
            check.Activate();
            /*// Detener cualquier movimiento y desactivar f�sica si tiene un Rigidbody
            if (rb != null)
            {
                rb.velocity = Vector3.zero;  // Detener movimiento
                rb.angularVelocity = Vector3.zero;  // Detener rotaci�n
                rb.isKinematic = true; // Desactivar f�sica
            }

            // Teletransportar al checkpoint usando Coroutine
            StartCoroutine(TeleportToCheckpoint(checkPoint));

            // Rehabilitar la f�sica despu�s del teletransporte
            if (rb != null)
            {
                rb.isKinematic = false; // Rehabilitar f�sica
            }

            Debug.Log("Teletransportado a la posici�n del checkpoint: " + checkPoint);
        }*/
        }

        /*private IEnumerator TeleportToCheckpoint(Vector3 targetPosition)
        {
            // Opcional: Agregar una peque�a demora o efectos si es necesario
            yield return null;  // Esperar un frame para asegurar que la f�sica est� desactivada

            // Teletransportar al jugador
            transform.position = targetPosition;*/
    }
}
