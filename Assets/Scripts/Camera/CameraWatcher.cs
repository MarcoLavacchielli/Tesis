using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWatcher : MonoBehaviour
{
    public float rotationSpeed = 50f; // Velocidad de rotaci�n en grados por segundo
    private float currentYRotation = 0f;
    private int rotationDirection = 1; // 1 para rotar hacia adelante, -1 para rotar hacia atr�s

    void Update()
    {
        RotateObject();
    }

    void RotateObject()
    {
        // Calcula la cantidad de rotaci�n para este frame
        float rotationAmount = rotationSpeed * Time.deltaTime * rotationDirection;

        // Aplica la rotaci�n al objeto
        transform.Rotate(0, rotationAmount, 0);

        // Actualiza la rotaci�n actual
        currentYRotation += rotationAmount;

        // Verifica si hemos alcanzado los l�mites de rotaci�n y cambia la direcci�n si es necesario
        if (currentYRotation >= 50f || currentYRotation <= -50f)
        {
            rotationDirection *= -1;
            // Clampa la rotaci�n actual al l�mite
            currentYRotation = Mathf.Clamp(currentYRotation, -50f, 50f);
        }
    }
}
