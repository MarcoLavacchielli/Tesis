using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovementGrappling pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float overshootYAxis;
    public float postGrappleForwardVelocity = 10f; // Velocidad para el empuje hacia adelante después del grapple

    private Vector3 grapplePoint;
    private bool isGrappling = false;

    [Header("Movement")]
    public float grappleSpeed = 10f; // Velocidad durante el grapple
    private Vector3 initialPosition;
    private float journeyLength;
    private float startTime;
    private bool reachedTarget = false;

    private Rigidbody rb;

    private void Start()
    {
        pm = GetComponent<PlayerMovementGrappling>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1)) // Click derecho para empezar el grapple
        {
            StartGrapple();
        }

        if (isGrappling)
        {
            MoveTowardsGrapplePoint();
        }
    }

    private void StartGrapple()
    {
        if (isGrappling) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
        }
        else
        {
            return; // No hay un punto válido de enganche
        }

        isGrappling = true;
        pm.freeze = false; // Deshabilitamos el freeze

        initialPosition = transform.position;
        journeyLength = Vector3.Distance(initialPosition, grapplePoint);
        startTime = Time.time;

        lr.enabled = true;
        //lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void MoveTowardsGrapplePoint()
    {
        float distanceCovered = (Time.time - startTime) * grappleSpeed;

        if (distanceCovered >= journeyLength)
        {
            distanceCovered = journeyLength;
            reachedTarget = true;
        }

        // Reduce la velocidad a medida que te acercas al objetivo
        float fractionOfJourney = Mathf.SmoothStep(0, 1, distanceCovered / journeyLength);

        Vector3 targetPosition = Vector3.Lerp(initialPosition, grapplePoint, fractionOfJourney);
        transform.position = targetPosition;

        if (reachedTarget)
        {
            ApplyGrappleCompletion();
        }
    }


    private void ApplyGrappleCompletion()
    {
        StopGrapple();

        if (rb != null)
        {
            StartCoroutine(SmoothApplyForwardMomentum());
        }
    }

    private IEnumerator SmoothApplyForwardMomentum()
    {
        Vector3 grappleDirection = (grapplePoint - initialPosition).normalized;
        Vector3 forwardMomentum = grappleDirection * postGrappleForwardVelocity;

        // Suavizamos la aplicación de la fuerza en 0.2 segundos
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, forwardMomentum + Vector3.down * pm.jumpForce * 0.5f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Aseguramos la velocidad final
        rb.velocity = forwardMomentum + Vector3.down * pm.jumpForce * 0.5f;
    }


    public void StopGrapple()
    {
        pm.freeze = false; // Aseguramos que el jugador no esté congelado
        isGrappling = false;

        lr.enabled = false; // Desactivamos el LineRenderer
        reachedTarget = false;
    }

    public bool IsGrappling()
    {
        return isGrappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
