using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockColumn : MonoBehaviour
{
    public GameObject[] CarasDeLaColumna; // Array con las caras del nonágono
    public float rotationSpeed = 50f;    // Velocidad de rotación
    public float restartSpeed = 50f;    // velocidad de cuando te equivocas
    public Transform filaCentral;        // Transform que representa la fila central
    public Material correctMaterial;     // Material para la cara correcta
    private bool isStopped = false;      // Indica si esta columna ya está resuelta

    private int codigoCorrecto;          // Índice de la cara correcta (ahora se asigna aleatoriamente)
    private Transform CorrectFace;

    void Start()
    {
        // Asignar aleatoriamente el índice del código correcto
        codigoCorrecto = Random.Range(0, CarasDeLaColumna.Length);

        // Cambiar el material de la cara correcta al inicio
        if (CarasDeLaColumna.Length > 0 && codigoCorrecto >= 0 && codigoCorrecto < CarasDeLaColumna.Length)
        {
            Renderer renderer = CarasDeLaColumna[codigoCorrecto].GetComponent<Renderer>();
            if (renderer != null && correctMaterial != null)
            {
                renderer.material = correctMaterial;
            }
            else
            {
                Debug.LogWarning("El Renderer o el Material de la cara correcta no están asignados.");
            }
            CorrectFace = CarasDeLaColumna[codigoCorrecto].transform;
        }
        else
        {
            Debug.LogError("El índice del código correcto está fuera de rango o el array está vacío.");
        }
    }

    void Update()
    {
        if (!isStopped)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    public bool TryStopColumn()
    {
        if (Vector3.Distance(CorrectFace.position, filaCentral.position) < 0.1f)
        {
            isStopped = true; // Detiene la columna
            rotationSpeed = 0; // Deja de rotar
            return true; // Indica que la columna fue resuelta correctamente
        }
        return false; // Indica que el jugador falló
    }

    public void ResetColumn()
    {
        isStopped = false; // Reactiva la columna
        rotationSpeed = restartSpeed; // Restaura la velocidad de rotación
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}
