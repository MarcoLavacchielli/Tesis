using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VigilantCam : MonoBehaviour
{
    public float rotationSpeed = 30f; // Velocidad de rotación en grados por segundo
    [SerializeField] float _radius;
    public Transform PlayerTransform;
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
    public void OnDrawGizmos()
    {
        

        Gizmos.color = Color.green;
        Gizmos.DrawLine(PlayerTransform.position + PlayerTransform.up * 0.5f, PlayerTransform.position + PlayerTransform.up * 0.5f + PlayerTransform.right * _radius);
        Gizmos.DrawLine(PlayerTransform.position - PlayerTransform.up * 0.5f, PlayerTransform.position - PlayerTransform.up * 0.5f + PlayerTransform.right * _radius);

    }
}
