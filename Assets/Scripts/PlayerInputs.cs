using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputs : MonoBehaviour
{
    public event Action MovementFuncInputs = delegate { };
    public event Action ShootPistolFuncInput = delegate { };
    private void FixedUpdate()
    {
        MovementFuncInputs();


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootPistolFuncInput();

        }
    }
}
