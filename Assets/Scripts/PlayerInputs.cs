using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.VFX;

public class PlayerInputs : MonoBehaviour
{
    public event Action MovementFuncInputs = delegate { };
    public event Action ShootPistolFuncInput = delegate { };
    public BulletSlider ReactivationTimeScript;
    public bool canShoot=true;

    public VisualEffect shootVisualEffect; //Particulas
    /*private void FixedUpdate()
    {
        MovementFuncInputs();


    }
    */
    private void Start()
    {
        canShoot=true;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)&& canShoot==true)
        {
            ShootPistolFuncInput();
            //Debug.Log("shoot");
            ReactivationTimeScript.ReactivationTimeTrigger();
            canShoot = false;
            shootVisualEffect.Play();
        }
    }
}
