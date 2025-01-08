using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitLaser : MonoBehaviour
{
    // Velocidad de rotación, configurable desde el editor.
    public float rotationSpeed = 10f;

    void Update()
    {
        // Calcula la rotación basada en el tiempo y la velocidad.
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Aplica la rotación en el eje X.
        transform.Rotate(rotationAmount, 0f, 0f);
    }
}
