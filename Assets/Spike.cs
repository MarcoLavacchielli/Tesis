using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : Trap
{
    public int damageAmount;

    private void OnCollisionEnter(Collision other)
    {
        Activate();
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
