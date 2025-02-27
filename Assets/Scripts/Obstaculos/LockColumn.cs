using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockColumn : MonoBehaviour
{
    public GameObject[] CarasDeLaColumna; // Array con las caras del non�gono
    public float rotationSpeed = 50f;    // Velocidad de rotaci�n
    public float restartSpeed = 50f;    // velocidad de cuando te equivocas
    public Transform filaCentral;        // Transform que representa la fila central
    public Material correctMaterial;     // Material para la cara correcta ---Amarillo
    public Material wellIntroducedCodeMaterial; //VERDE
    private bool isStopped = false;      // Indica si esta columna ya est� resuelta

    private int codigoCorrecto;          // �ndice de la cara correcta (ahora se asigna aleatoriamente)
    private Transform CorrectFace;
    private Renderer correctFaceRenderer;

    void Start()
    {
        // Asignar aleatoriamente el �ndice del c�digo correcto
        codigoCorrecto = Random.Range(0, CarasDeLaColumna.Length);

        // Cambiar el material de la cara correcta al inicio
        if (CarasDeLaColumna.Length > 0 && codigoCorrecto >= 0 && codigoCorrecto < CarasDeLaColumna.Length)
        {
            Renderer renderer = CarasDeLaColumna[codigoCorrecto].GetComponent<Renderer>();
            if (renderer != null && correctMaterial != null)
            {
                renderer.material = correctMaterial;
                correctFaceRenderer = renderer;
            }
            else
            {
                Debug.LogWarning("El Renderer o el Material de la cara correcta no est�n asignados.");
            }
            CorrectFace = CarasDeLaColumna[codigoCorrecto].transform;
        }
        else
        {
            Debug.LogError("El �ndice del c�digo correcto est� fuera de rango o el array est� vac�o.");
        }
    }

    void Update()
    {
        if (!isStopped)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
    public void changeColorToGreen()
    {
        Renderer renderer = CarasDeLaColumna[codigoCorrecto].GetComponent<Renderer>();
        if (renderer != null && correctMaterial != null)
        {
            renderer.material = wellIntroducedCodeMaterial;
        }


    }

    public bool TryStopColumn()
    {
        if (Vector3.Distance(CorrectFace.position, filaCentral.position) < 0.1f)
        {
           changeColorToGreen();
            isStopped = true; // Detiene la columna
            rotationSpeed = 0; // Deja de rotar
            return true; // Indica que la columna fue resuelta correctamente
        }
        return false; // Indica que el jugador fall�
    }

    public void ResetColumn()
    {
        isStopped = false; // Reactiva la columna
        rotationSpeed = restartSpeed; // Restaura la velocidad de rotaci�n
        Renderer renderer = CarasDeLaColumna[codigoCorrecto].GetComponent<Renderer>();
        if (renderer != null && correctMaterial != null)
        {
            renderer.material = correctMaterial;
        }
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}
