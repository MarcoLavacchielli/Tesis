using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserx4V2 : MonoBehaviour
{
    public float velocidadRotacion;

    void Update()
    {
        transform.Rotate(Vector3.right, velocidadRotacion * Time.deltaTime);
    }
}
