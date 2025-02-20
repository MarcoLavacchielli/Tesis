using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    public float inactivityThreshold = 1f; // Tiempo en segundos para desactivar
    private Vector3 lastPosition;
    private float inactivityTimer;
    private bool isInactive;

    void Start()
    {
        lastPosition = transform.position;
        inactivityTimer = 0f;
        isInactive = false;
    }

    void Update()
    {
        // Comprobamos si el objeto se ha movido
        if (transform.position != lastPosition)
        {
            // Si se ha movido, reseteamos el temporizador
            inactivityTimer = 0f;
            if (isInactive)
            {
                gameObject.SetActive(true); // Reactivamos el objeto si estaba inactivo
                isInactive = false;
            }
        }
        else
        {
            // Si no se ha movido, aumentamos el temporizador
            inactivityTimer += Time.deltaTime;
        }

        // Si el objeto ha estado inactivo durante el tiempo especificado, lo desactivamos
        if (inactivityTimer >= inactivityThreshold && !isInactive)
        {
            gameObject.SetActive(false);
            isInactive = true;
        }

        // Guardamos la última posición para la próxima actualización
        lastPosition = transform.position;
    }
}