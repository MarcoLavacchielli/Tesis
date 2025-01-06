using UnityEngine;

public class TeleportOnTrigger : MonoBehaviour
{
    public Rigidbody targetRigidbody; // El Rigidbody que vas a teletransportar
    public Vector3 targetPosition;   // La posición a la que quieres teletransportarlo
    public string playerLayerName = "Player"; // Nombre de la capa del Player

    /*void Update()
    {
        // Detecta si se presiona la tecla "K"
        if (Input.GetKeyDown(KeyCode.K))
        {
            TeleportObject();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entró al trigger tiene la capa "Player"
        if (other.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            TeleportObject();
        }
    }

    void TeleportObject()
    {
        if (targetRigidbody != null)
        {
            // Desactiva temporalmente las colisiones del Rigidbody para evitar problemas al teletransportar
            targetRigidbody.detectCollisions = false;

            // Cambia la posición del Rigidbody
            targetRigidbody.position = targetPosition;

            // Reactiva las colisiones
            targetRigidbody.detectCollisions = true;

            Debug.Log($"Objeto teletransportado a {targetPosition}");
        }
        else
        {
            Debug.LogWarning("No se asignó un Rigidbody de destino.");
        }
    }
}
