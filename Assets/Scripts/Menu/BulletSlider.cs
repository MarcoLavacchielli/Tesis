using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSlider : MonoBehaviour
{
    public float MaxEnergy = 1;
    public float MinEnergy = 0;
    public float IncreaseSpeedOfEnergy = 1f;
    public float DecreaseSpeedOfEnergy = 1f;
    public Slider EnergySliderBar;
    public float currentEnergyValue;
    public PlayerInputs playerInputs;
    public bool callPlayerInputs;

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
    public void ReactivationTimeTrigger()
    {
        currentEnergyValue = MinEnergy;
        callPlayerInputs=true;
    }

    private void UpdateEnergyBar()
    {
        EnergySliderBar.value = currentEnergyValue / MaxEnergy;
    }
    private void Update()
    {
        if (currentEnergyValue < MaxEnergy)
        {
            EnergyConsumptionFunction();


        }
        if (callPlayerInputs == false) return;
        if (callPlayerInputs == true && currentEnergyValue >= MaxEnergy)
        {
            playerInputs.canShoot = true;
        }
    }

}
