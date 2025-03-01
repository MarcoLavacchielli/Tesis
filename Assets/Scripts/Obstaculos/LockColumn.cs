using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockColumn : MonoBehaviour
{
    public GameObject[] CarasDeLaColumna; // Array con las caras del nonágono
    public float rotationSpeed = 50f;    // Velocidad de rotación
    public float restartSpeed = 50f;    // Velocidad cuando se reinicia
    public Transform filaCentral;        // Transform que representa la fila central
    public Material correctMaterial;     // Material para la cara correcta (Amarillo)
    public Material wellIntroducedCodeMaterial; // Material cuando el código es correcto (Verde)
    public Material incorrectMaterial; // Material para las caras incorrectas (Rojo)
    private bool isStopped = false;      // Indica si esta columna ya está resuelta

    private int codigoCorrecto;          // Índice de la cara correcta
    private Transform CorrectFace;
    private Renderer correctFaceRenderer;

    void Start()
    {
        InicializarCandado();
    }

    void OnEnable()
    {
        ResetearMateriales();
        InicializarCandado();
    }

    void InicializarCandado()
    {
        if (CarasDeLaColumna.Length == 0)
        {
            Debug.LogError("El array de caras de la columna está vacío.");
            return;
        }

        // Si el código ya fue asignado, no lo reasignamos
        if (codigoCorrecto == 0 && correctFaceRenderer == null)
        {
            codigoCorrecto = Random.Range(0, CarasDeLaColumna.Length);
        }

        // Asigna el material correcto solo a la cara correcta
        for (int i = 0; i < CarasDeLaColumna.Length; i++)
        {
            Renderer renderer = CarasDeLaColumna[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = (i == codigoCorrecto) ? correctMaterial : incorrectMaterial;
            }
        }

        correctFaceRenderer = CarasDeLaColumna[codigoCorrecto].GetComponent<Renderer>();
        CorrectFace = CarasDeLaColumna[codigoCorrecto].transform;
    }

    void ResetearMateriales()
    {
        for (int i = 0; i < CarasDeLaColumna.Length; i++)
        {
            Renderer renderer = CarasDeLaColumna[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = incorrectMaterial;
            }
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
        if (correctFaceRenderer != null && wellIntroducedCodeMaterial != null)
        {
            correctFaceRenderer.material = wellIntroducedCodeMaterial;
        }
    }

    public bool TryStopColumn()
    {
        if (Vector3.Distance(CorrectFace.position, filaCentral.position) < 0.1f)
        {
            changeColorToGreen();
            isStopped = true;
            rotationSpeed = 0;
            return true;
        }
        return false;
    }

    public void ResetColumn()
    {
        isStopped = false;
        rotationSpeed = restartSpeed;
        for (int i = 0; i < CarasDeLaColumna.Length; i++)
        {
            Renderer renderer = CarasDeLaColumna[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = (i == codigoCorrecto) ? correctMaterial : incorrectMaterial;
            }
        }
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}