using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{
    public Animator spikeTrap;

    public int damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spikeTrap.Play("Spike Trap (Abrir)");
            Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        spikeTrap.Play("Spike Trap (Cerrar)");
    }

    public override void Activate()
    {
        Debug.Log("Spike trap activated!");

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
