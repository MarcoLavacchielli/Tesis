using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
