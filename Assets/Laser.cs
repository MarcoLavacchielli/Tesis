using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Trap
{
    public int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Activate();
        }
    }

    public override void Activate()
    {
        Debug.Log("Player touch laser!");

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
