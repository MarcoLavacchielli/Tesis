using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWatcher : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de rotación en grados por segundo
    private float currentYRotation = 0f;
    private int rotationDirection = 1; // 1 para rotar hacia adelante, -1 para rotar hacia atrás

    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        // Calcula la cantidad de rotación para este frame
        float rotationAmount = rotationSpeed * Time.deltaTime * rotationDirection;

        // Aplica la rotación al objeto
        transform.Rotate(0, rotationAmount, 0);

        // Actualiza la rotación actual
        currentYRotation += rotationAmount;

        // Verifica si hemos alcanzado los límites de rotación y cambia la dirección si es necesario
        if (currentYRotation >= 50f || currentYRotation <= -50f)
        {
            rotationDirection *= -1;
            // Clampa la rotación actual al límite
            currentYRotation = Mathf.Clamp(currentYRotation, -50f, 50f);
        }
    }
}
