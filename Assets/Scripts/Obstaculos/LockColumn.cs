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

   /* private float anglePerFace;          // Ángulo que ocupa cada cara

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

        // Calcular el ángulo que ocupa cada cara
        anglePerFace = 360f / CarasDeLaColumna.Length;
    }

    void Update()
    {
        if (!isStopped)
        {
            // Rotación continua
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            // Detectar la cara alineada con la fila central
            int currentFace = GetCurrentFaceIndex();
            if (Input.GetKeyDown(KeyCode.Return) && currentFace == codigoCorrecto)
            {
                isStopped = true; // Detener la rotación
                rotationSpeed = 0;
                NotifyNextColumn(); // Notificar que puede avanzar
            }
        }
    }

    private int GetCurrentFaceIndex()
    {
        // Obtener el ángulo actual del nonágono
        float currentRotation = transform.eulerAngles.y;

        // Asegurarnos de que el ángulo esté en el rango 0-360
        currentRotation = (currentRotation + 360f) % 360f;

        // Determinar qué cara está en la posición central
        int faceIndex = Mathf.RoundToInt(currentRotation / anglePerFace) % CarasDeLaColumna.Length;
        return faceIndex;
    }

    private void NotifyNextColumn()
    {
        // Lógica para activar la siguiente columna o verificar desbloqueo
        Debug.Log("Columna resuelta. Procede a la siguiente.");
    }
    */

    
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



            /* // Rotación continua
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

                // Detección de la cara en la fila central
                foreach (GameObject cara in CarasDeLaColumna)
                {
                    if (IsInCentralRow(cara))
                    {
                        // Espera a la tecla Enter
                        if (Input.GetKeyDown(KeyCode.Return) && IsCorrectDigit(cara))
                        {
                            isStopped = true; // Detiene la columna
                            rotationSpeed = 0; // Deja de rotar
                            NotifyNextColumn(); // Notifica que la siguiente columna puede activarse
                        }
                    }
                }
            }*/
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


        /*
        // Verifica si la cara está alineada con la fila central
            float distance = Mathf.Abs(cara.transform.position.z - filaCentral.position.z);
        if (distance < 0.002f)
        {
            float distancex = Mathf.Abs(cara.transform.position.x - filaCentral.position.x);
        }
            return distance < 0.002f; // Tolerancia ajustable*/
        }

        private bool IsCorrectDigit(GameObject cara)
        {
            // Comprueba si la cara actual es la correcta
            int index = System.Array.IndexOf(CarasDeLaColumna, cara);
            return index == codigoCorrecto;
        }

        private void NotifyNextColumn()
        {
            // Lógica para activar la siguiente columna o verificar desbloqueo
            Debug.Log("Columna resuelta. Procede a la siguiente.");
        }

}
