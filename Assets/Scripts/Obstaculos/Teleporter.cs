using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 teleportPos;

    [SerializeField] private bool toco=false;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el collider que entró es el jugador
        if (other.CompareTag("Player"))
        {
            toco = true;
            // Teletransporta al jugador a la posición deseada
            player.transform.position = teleportPos;
        }
    }
}
