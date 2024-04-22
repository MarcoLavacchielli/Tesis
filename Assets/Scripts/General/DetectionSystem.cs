using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSystem : MonoBehaviour
{
    public EnergyBar EnergyBarScript;
    private bool BeingWatch;

    private void Update()
    {
        if (BeingWatch==true)
        {
            EnergyBarScript.EnergyConsumptionFunction();
        }
    }
}
