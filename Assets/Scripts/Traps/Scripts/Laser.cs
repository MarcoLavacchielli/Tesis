using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Trap
{
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

        EnergyBar energyBar = FindObjectOfType<EnergyBar>();
        if (energyBar != null)
        {
            energyBar.EnergyConsumptionFunction();
        }
    }
}
