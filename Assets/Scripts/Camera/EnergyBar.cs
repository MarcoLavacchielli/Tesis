using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Facundo Sebastian Tisera
public class EnergyBar : MonoBehaviour//Script donde se carga y se descarga la barra de energía del Hunter
{
    public float MaxEnergy = 1;
    public float MinEnergy = 0;
    public float DecreaseSpeedOfEnergy =0.05f;
    public float IncreasSpeedOfEnergy = 0.08f;
    public Image EnergyImageBar;
    public float currentEnergyValue;
   

    private void Start()
    {
        currentEnergyValue = MaxEnergy;
        
    }
    
    

    public void BeingWatchEnergyConsumption()
    {
        currentEnergyValue = Mathf.Lerp(currentEnergyValue, MinEnergy, DecreaseSpeedOfEnergy * Time.deltaTime);

        EnergyImageBar.fillAmount = currentEnergyValue / MaxEnergy;
    }

    public void EnergyRecoveryFunction()
    {
        currentEnergyValue = Mathf.Lerp(currentEnergyValue, MaxEnergy, IncreasSpeedOfEnergy * Time.deltaTime);
        
        EnergyImageBar.fillAmount = currentEnergyValue / MaxEnergy;
    }
}
