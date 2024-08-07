using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Laser : Trap
{
    public PlayerMovementAdvanced playerMovement;
    private bool isSlowed = false;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        EnergyBar energyBar = FindAnyObjectByType<EnergyBar>();
        if (energyBar != null)
        {
            energyBar.EnergyConsumptionFunction();
        }

        if (!isSlowed)
        {
            StartCoroutine(SlowDownAndRestore());
        }
    }

    IEnumerator SlowDownAndRestore()
    {
        isSlowed = true;

        playerMovement.walkSpeed /= 3f;
        playerMovement.sprintSpeed /= 3f;
        playerMovement.slideSpeed /= 3f;
        playerMovement.wallrunSpeed /= 3f;
        playerMovement.climbSpeed /= 3f;

        yield return new WaitForSeconds(2f);

        playerMovement.walkSpeed *= 3f;
        playerMovement.sprintSpeed *= 3f;
        playerMovement.slideSpeed *= 3f;
        playerMovement.wallrunSpeed *= 3f;
        playerMovement.climbSpeed *= 3f;

        isSlowed = false;
    }
}