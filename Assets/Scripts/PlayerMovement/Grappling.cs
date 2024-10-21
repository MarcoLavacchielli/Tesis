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
        rb = GetComponent<Rigidbody>(); // Tomamos el Rigidbody una vez
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
        //lr.SetPosition(1, grapplePoint);
    }

    private void MoveTowardsGrapplePoint()
    {
        float distanceCovered = (Time.time - startTime) * grappleSpeed;

        if (distanceCovered >= journeyLength)
        {
            distanceCovered = journeyLength;
            reachedTarget = true;
        }

        float fractionOfJourney = distanceCovered / journeyLength;

        Vector3 targetPosition = Vector3.Lerp(initialPosition, grapplePoint, fractionOfJourney);
        transform.position = targetPosition;

        if (reachedTarget)
        {
            ApplyGrappleCompletion();
        }
    }

    private void ApplyGrappleCompletion()
    {
        StopGrapple(); // Paramos el grapple y aplicamos la física

        if (rb != null)
        {
            // Calculamos la dirección en la que el jugador estaba yendo antes de terminar el grapple
            Vector3 grappleDirection = (grapplePoint - initialPosition).normalized;

            // Aplicamos la velocidad constante hacia adelante, usando la dirección hacia el punto de enganche
            Vector3 forwardMomentum = grappleDirection * postGrappleForwardVelocity;

            // Añadimos también el efecto de la gravedad para que caiga naturalmente
            rb.velocity = forwardMomentum + Vector3.down * pm.jumpForce * 0.5f;
        }
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