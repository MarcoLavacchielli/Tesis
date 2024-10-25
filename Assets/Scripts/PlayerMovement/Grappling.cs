using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovementGrappling pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LayerMask avoidLayer;
    public Image grappleIndicator;

    public PauseMenu pauseMenu;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float overshootYAxis;

    private Transform grappleTarget; // Guardamos el transform del objeto con el que hacemos contacto
    private bool isGrappling = false;

    [Header("Movement")]
    public float grappleSpeed = 10f;
    private Vector3 initialPosition;
    private float journeyLength;
    private float startTime;
    private bool reachedTarget = false;

    private Rigidbody rb;

    private void Start()
    {
        pm = GetComponent<PlayerMovementGrappling>();
        rb = GetComponent<Rigidbody>();
        grappleIndicator.color = Color.white;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        if (pauseMenu.isPaused) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartGrapple();
        }

        if (isGrappling)
        {
            MoveTowardsGrapplePoint();
        }
        else
        {
            CheckGrappleable();
        }
    }

    private void CheckGrappleable()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            if (!Physics.Linecast(cam.position, hit.point, avoidLayer))
            {
                grappleIndicator.color = Color.green;
            }
            else
            {
                grappleIndicator.color = Color.white;
            }
        }
        else
        {
            grappleIndicator.color = Color.white;
        }
    }

    private void StartGrapple()
    {
        if (isGrappling) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            if (!Physics.Linecast(cam.position, hit.point, avoidLayer))
            {
                grappleTarget = hit.transform; // Guardamos el transform del objeto con el que nos enganchamos
                grappleIndicator.color = Color.green;
            }
            else
            {
                grappleIndicator.color = Color.white;
                return;
            }
        }
        else
        {
            grappleIndicator.color = Color.white;
            return;
        }

        isGrappling = true;
        pm.freeze = false;

        initialPosition = transform.position;
        journeyLength = Vector3.Distance(initialPosition, grappleTarget.position); // Usamos la posición del objeto dinámico
        startTime = Time.time;
    }

    private void MoveTowardsGrapplePoint()
    {
        // Actualizamos el punto del gancho cada frame, sigue la posición del objeto en movimiento
        Vector3 grapplePoint = grappleTarget.position;

        Vector3 direction = (grapplePoint - transform.position).normalized;
        rb.velocity = direction * grappleSpeed;

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

    [Header("Post Grapple Settings")]
    public float upwardImpulse = 3f;
    public float postGrappleForwardVelocity = 10f;

    private void ApplyGrappleCompletion()
    {
        pm.freeze = false;
        isGrappling = false;
        StopGrappleUnique();

        if (rb != null)
        {
            Vector3 grappleDirection = (grappleTarget.position - initialPosition).normalized;

            Vector3 forwardMomentum = grappleDirection * postGrappleForwardVelocity;
            Vector3 upwardMomentum = Vector3.up * upwardImpulse;

            rb.velocity = forwardMomentum + upwardMomentum;
        }
    }

    public void StopGrapple()
    {
        pm.freeze = false;
        isGrappling = false;
        grappleIndicator.color = Color.white;
        reachedTarget = false;
    }

    public void StopGrappleUnique()
    {
        grappleIndicator.color = Color.white;
        reachedTarget = false;
    }

    public void InterruptGrapple()
    {
        if (isGrappling)
        {
            StopGrapple();
        }
    }

    public bool IsGrappling()
    {
        return isGrappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grappleTarget.position; // Devolvemos la posición actual del objeto
    }
}
