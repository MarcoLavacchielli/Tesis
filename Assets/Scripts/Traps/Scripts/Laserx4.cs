using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserx4 : MonoBehaviour
{
    public float velocidadRotacion;

    void Update()
    {
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime);
    }
}
