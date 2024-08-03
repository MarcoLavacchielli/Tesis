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

    public Material targetMaterial; // Reference to the material with the emissive color
    private Color minEnergyColor = Color.red; // Emissive color when energy is 0
    private Color maxEnergyColor = Color.cyan; // Emissive color when energy is 1

    private void Start()
    {
        EnergySliderBar.maxValue = MaxEnergy;
        currentEnergyValue = MinEnergy;
        UpdateEnergyBar();
        UpdateEmissiveColor();
    }

    public void EnergyConsumptionFunction()
    {
        currentEnergyValue = Mathf.Min(currentEnergyValue + IncreaseSpeedOfEnergy * Time.deltaTime, MaxEnergy);
        UpdateEnergyBar();
        UpdateEmissiveColor();
    }

    public void EnergyRecoveryFunction()
    {
        currentEnergyValue = Mathf.Max(currentEnergyValue - DecreaseSpeedOfEnergy * Time.deltaTime, MinEnergy);
        UpdateEnergyBar();
        UpdateEmissiveColor();
    }

    public void ReactivationTimeTrigger()
    {
        currentEnergyValue = MinEnergy;
        callPlayerInputs = true;
        UpdateEmissiveColor();
    }

    private void UpdateEnergyBar()
    {
        EnergySliderBar.value = currentEnergyValue / MaxEnergy;
    }

    private void UpdateEmissiveColor()
    {
        if (targetMaterial != null)
        {
            Color emissiveColor = Color.Lerp(minEnergyColor, maxEnergyColor, currentEnergyValue / MaxEnergy);
            targetMaterial.SetColor("_EmissiveColor", emissiveColor);
        }
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
