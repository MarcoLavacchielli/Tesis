using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
    public LockColumn[] columnas; // Array de columnas
    private int columnaActual = 0; // �ndice de la columna activa

    void Start()
    {
        ActivarColumnaActual();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ProcesarColumnaActual();
        }
    }

    private void ProcesarColumnaActual()
    {
        if (columnaActual >= 0 && columnaActual < columnas.Length)
        {
            if (columnas[columnaActual].TryStopColumn())
            {
                // Columna resuelta correctamente
                Debug.Log("C�digo correcto. Avanzando a la siguiente columna.");
                columnaActual++;
                if (columnaActual >= columnas.Length)
                {
                    // Puzzle completado
                    Debug.Log("�Puzzle resuelto! Desbloqueando...");
                }
                else
                {
                    ActivarColumnaActual();
                }
            }
            else
            {
                // C�digo incorrecto, retroceder una columna
                Debug.Log("C�digo incorrecto. Retrocediendo a la columna anterior.");
                if (columnaActual > 0)
                {
                    columnaActual--;
                }
                ActivarColumnaActual();
            }
        }
    }

    private void ActivarColumnaActual()
    {
        for (int i = 0; i < columnas.Length; i++)
        {
            if (i == columnaActual)
            {
                columnas[i].ResetColumn();
            }
        }
    }





}
