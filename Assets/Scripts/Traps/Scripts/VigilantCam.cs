using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VigilantCam : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación en grados por segundo

    private bool movingForward = true;

    void Update()
    {
        // Si estamos moviéndonos hacia adelante
        if (movingForward)
        {
            // Rotar hacia la derecha (45 grados en el eje Y)
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Si hemos alcanzado los 45 grados, cambiar dirección
            if (transform.rotation.eulerAngles.y >= 90)
            {
                movingForward = false;
            }
        }
        else
        {
            // Rotar hacia la izquierda (-45 grados en el eje Y)
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);

            // Si hemos alcanzado los -45 grados, cambiar dirección
            if (transform.rotation.eulerAngles.y <= 0)
            {
                movingForward = true;
            }
        }
    }
}
