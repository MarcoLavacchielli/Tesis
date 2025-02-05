using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    AudioManager audioM;

    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    public Transform playercamToTilt; // Referencia a la cámara para inclinar
    private Rigidbody rb;
    private PlayerMovementAdvanced pm;
    private PlayerMovementGrappling pg;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    [Header("Camera Tilt")]
    public float tiltAngle = 10f; // Ángulo de inclinación durante el slide
    public float tiltSpeed = 5f; // Velocidad de interpolación
    private float currentTilt = 0f;

    private CameraTiltController camTilt;

    private void Awake()
    {
        audioM = FindObjectOfType<AudioManager>();
        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
        pg = GetComponent<PlayerMovementGrappling>();

        startYScale = playerObj.localScale.y;
        camTilt = FindObjectOfType<CameraTiltController>(); // Referencia al nuevo controlador de inclinación
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey) && pm.sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();

        HandleCameraTilt(); // Aplicar inclinación de la cámara
    }

    private void StartSlide()
    {
        if (pm.wallrunning || pg.activeGrapple) return;

        pm.sliding = true;
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        audioM.PlaySfx(4);
        slideTimer = maxSlideTime;

        camTilt.SetTilt(-tiltAngle); // Aplica inclinación
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);

        camTilt.SetTilt(0f); // Restaura la inclinación
    }

    private void HandleCameraTilt()
    {
        float targetTilt = pm.sliding ? -tiltAngle : 0f;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        playercamToTilt.localRotation = Quaternion.Euler(0, 0, currentTilt);
    }
}
