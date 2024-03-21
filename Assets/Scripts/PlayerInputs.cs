using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInputs : MonoBehaviour
{
    public event Action MovementFuncInputs = delegate { };



    private void FixedUpdate()
    {
        MovementFuncInputs();
    }
}
