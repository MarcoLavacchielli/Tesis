using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTiltController : MonoBehaviour
{
    public Transform playercamToTilt;

    private float currentTilt = 0f;
    private float targetTilt = 0f;
    public float tiltSpeed = 5f;

    public void SetTilt(float tilt)
    {
        targetTilt = tilt;
    }

    private void Update()
    {
        // Aplicar suavemente la inclinación deseada
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        playercamToTilt.localRotation = Quaternion.Euler(0, 0, currentTilt);
    }
}
