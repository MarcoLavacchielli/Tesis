using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 teleportPos;

    [SerializeField] private bool toco = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            toco = true;
            player.transform.position = teleportPos;
        }
    }
}