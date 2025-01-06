using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockColumn : MonoBehaviour
{
    public GameObject[] CarasDeLaColumna; // Array con las caras del nonágono
    public float rotationSpeed = 50f;    // Velocidad de rotación
    public int codigoCorrecto;           // Índice de la cara correcta (0 a 8)
    public Transform filaCentral;        // Transform que representa la fila central
    public Material correctMaterial;     // Material para la cara correcta
    private bool isStopped = false;      // Indica si esta columna ya está resuelta

    public Transform CorrectFace;

    void Start()
    {
        // Cambiar el material de la cara correcta al inicio
        if (codigoCorrecto >= 0 && codigoCorrecto < CarasDeLaColumna.Length)
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
        }
        else
        {
            Debug.LogError("El índice del código correcto está fuera de rango.");
        }
        CorrectFace = CarasDeLaColumna[codigoCorrecto].transform;
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
        rotationSpeed = 50f; // Restaura la velocidad de rotación
    }

    public bool IsStopped()
    {
        return isStopped;
    }

    /*
        void Start()
        {
            // Cambiar el material de la cara correcta al inicio
            if (codigoCorrecto >= 0 && codigoCorrecto < CarasDeLaColumna.Length)
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
            }
            else
            {
                Debug.LogError("El índice del código correcto está fuera de rango.");
            }
          CorrectFace = CarasDeLaColumna[codigoCorrecto].gameObject.transform;

        }


    void Update()
    {
        if (!isStopped)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);


            if (Input.GetKeyDown(KeyCode.Return))
            {

                IsInCentralRow();
            
            }



          
        }
    }
        public void IsInCentralRow()
        {
            if(Vector3.Distance(CorrectFace.transform.position, filaCentral.transform.position) < 0.1f)
            {
                isStopped = true; // Detiene la columna
                rotationSpeed = 0; // Deja de rotar
                NotifyNextColumn(); // Notifica que la siguiente columna puede activarse
            }
        }

        private void NotifyNextColumn()
        {
            // Lógica para activar la siguiente columna o verificar desbloqueo
            Debug.Log("Columna resuelta. Procede a la siguiente.");
        }
    */
}
