using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWatcher : MonoBehaviour
{
    public float rotationSpeed = 50f;
    private float currentYRotation = 0f;
    private int rotationDirection = 1;
    private bool isPlayerDetected = false;

    void Update()
    {
        if (!isPlayerDetected)
        {
            RotateObject();
        }
    }

    void RotateObject()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime * rotationDirection;

        transform.Rotate(0, rotationAmount, 0);

        currentYRotation += rotationAmount;

        if (currentYRotation >= 50f || currentYRotation <= -50f)
        {
            rotationDirection *= -1;
            currentYRotation = Mathf.Clamp(currentYRotation, -50f, 50f);
        }
    }

    public void SetPlayerDetected(bool detected)
    {
        isPlayerDetected = detected;
    }
}