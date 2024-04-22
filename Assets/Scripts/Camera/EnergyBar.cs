using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    public float MaxEnergy = 1;
    public float MinEnergy = 0;
    public float IncreaseSpeedOfEnergy = 1f;
    public float DecreaseSpeedOfEnergy = 1f;
    public Slider EnergySliderBar;
    public float currentEnergyValue;

    private void Start()
    {
        EnergySliderBar.maxValue = MaxEnergy;
        currentEnergyValue = MinEnergy;
        UpdateEnergyBar();
    }

    public void EnergyConsumptionFunction()
    {
        currentEnergyValue = Mathf.Min(currentEnergyValue + IncreaseSpeedOfEnergy * Time.deltaTime, MaxEnergy);
        UpdateEnergyBar();
    }

    public void EnergyRecoveryFunction()
    {
        currentEnergyValue = Mathf.Max(currentEnergyValue - DecreaseSpeedOfEnergy * Time.deltaTime, MinEnergy);
        UpdateEnergyBar();
    }

    private void UpdateEnergyBar()
    {
        EnergySliderBar.value = currentEnergyValue / MaxEnergy;
    }
}