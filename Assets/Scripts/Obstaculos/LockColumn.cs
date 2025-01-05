using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockColumn : MonoBehaviour
{
    public GameObject[] CarasDeLaColumna; // Array con las caras del non�gono
    public float rotationSpeed = 50f;    // Velocidad de rotaci�n
    public int codigoCorrecto;           // �ndice de la cara correcta (0 a 8)
    public Transform filaCentral;        // Transform que representa la fila central
    public Material correctMaterial;     // Material para la cara correcta
    private bool isStopped = false;      // Indica si esta columna ya est� resuelta

    public Transform CorrectFace;

   /* private float anglePerFace;          // �ngulo que ocupa cada cara

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
                Debug.LogWarning("El Renderer o el Material de la cara correcta no est�n asignados.");
            }
        }
        else
        {
            Debug.LogError("El �ndice del c�digo correcto est� fuera de rango.");
        }

        // Calcular el �ngulo que ocupa cada cara
        anglePerFace = 360f / CarasDeLaColumna.Length;
    }

    void Update()
    {
        if (!isStopped)
        {
            // Rotaci�n continua
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

            // Detectar la cara alineada con la fila central
            int currentFace = GetCurrentFaceIndex();
            if (Input.GetKeyDown(KeyCode.Return) && currentFace == codigoCorrecto)
            {
                isStopped = true; // Detener la rotaci�n
                rotationSpeed = 0;
                NotifyNextColumn(); // Notificar que puede avanzar
            }
        }
    }

    private int GetCurrentFaceIndex()
    {
        // Obtener el �ngulo actual del non�gono
        float currentRotation = transform.eulerAngles.y;

        // Asegurarnos de que el �ngulo est� en el rango 0-360
        currentRotation = (currentRotation + 360f) % 360f;

        // Determinar qu� cara est� en la posici�n central
        int faceIndex = Mathf.RoundToInt(currentRotation / anglePerFace) % CarasDeLaColumna.Length;
        return faceIndex;
    }

    private void NotifyNextColumn()
    {
        // L�gica para activar la siguiente columna o verificar desbloqueo
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
                    Debug.LogWarning("El Renderer o el Material de la cara correcta no est�n asignados.");
                }
            }
            else
            {
                Debug.LogError("El �ndice del c�digo correcto est� fuera de rango.");
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



            /* // Rotaci�n continua
                transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

                // Detecci�n de la cara en la fila central
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
        // Verifica si la cara est� alineada con la fila central
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
            // L�gica para activar la siguiente columna o verificar desbloqueo
            Debug.Log("Columna resuelta. Procede a la siguiente.");
        }

}
