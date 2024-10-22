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
    public LayerMask avoidLayer; //Cosas que el raycast anula el gancho
    public Image grappleIndicator; // Circulito del hud

    public PauseMenu pauseMenu;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float overshootYAxis;

    private Vector3 grapplePoint;
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
        //Si esta pausado que vuelva (facu maneja todo desde la pausa pero este se hace desde aca porque se rompe)

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
                grapplePoint = hit.point;
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
        journeyLength = Vector3.Distance(initialPosition, grapplePoint);
        startTime = Time.time;
    }

    private void MoveTowardsGrapplePoint()
    {
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
    public float upwardImpulse = 3f; // AEmpuje hacia delante
    public float postGrappleForwardVelocity = 10f; // VEmpuje Hacia arriba


    private void ApplyGrappleCompletion()
    {
        pm.freeze = false; 
        isGrappling = false;
        StopGrappleUnique(); 

        if (rb != null)
        {
            // Direccion antes de tirar el gancho
            Vector3 grappleDirection = (grapplePoint - initialPosition).normalized;

            // Impulso constante hacia adelante en la dirección del grapple
            Vector3 forwardMomentum = grappleDirection * postGrappleForwardVelocity;

            // Impulso hacia arriba
            Vector3 upwardMomentum = Vector3.up * upwardImpulse; // Ajuste de variable 

            // Mismo impulso sin importar las circunstancias
            rb.velocity = forwardMomentum + upwardMomentum; // Valor FIJO
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

    //Corta gancho desde otros scripts
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
        return grapplePoint;
    }
}
