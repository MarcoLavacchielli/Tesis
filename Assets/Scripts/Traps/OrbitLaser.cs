using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLaser : MonoBehaviour
{
    // Velocidad de rotaci�n, configurable desde el editor.
    public float rotationSpeed = 10f;

    void Update()
    {
        // Calcula la rotaci�n basada en el tiempo y la velocidad.
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Aplica la rotaci�n en el eje X.
        transform.Rotate(rotationAmount, 0f, 0f);
    }
}
