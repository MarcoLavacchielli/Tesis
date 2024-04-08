using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : Trap
{
    public Collider activationCollider;
    public Animator spikeTrap;

    public int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger!");
        if (other.CompareTag("Player") && other == activationCollider)
        {
            spikeTrap.Play("Spike Trap (Abrir)");
            Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Not entered trigger!");
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
