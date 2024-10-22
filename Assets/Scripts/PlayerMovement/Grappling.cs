using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Aseg�rate de incluir esto para trabajar con UI

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovementGrappling pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LayerMask avoidLayer; // Capa para evitar grappling
    public Image grappleIndicator; // Referencia a la imagen que queremos cambiar

    public PauseMenu pauseMenu;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float overshootYAxis;
    public float postGrappleForwardVelocity = 10f; // Velocidad para el empuje hacia adelante despu�s del grapple

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
        grappleIndicator.color = Color.white; // Aseg�rate de que el color inicial sea blanco
    }

    private void Update()
    {
        if (pauseMenu.isPaused) return;

        if (Input.GetKeyDown(KeyCode.Mouse1)) // Click derecho para empezar el grapple
        {
            StartGrapple();
        }

        if (isGrappling)
        {
            MoveTowardsGrapplePoint();
        }
        else
        {
            // Comprobar si hay un objeto grapable en el rango
            CheckGrappleable();
        }
    }

    private void CheckGrappleable()
    {
        RaycastHit hit;
        // Realizamos el raycast para detectar un objeto grapable
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            // Verificamos si hay un objeto en la capa de evitar entre la c�mara y el objeto grapable
            if (!Physics.Linecast(cam.position, hit.point, avoidLayer))
            {
                // Si hay un objeto grappleable y no hay nada en la capa avoid, cambiamos el color a verde
                grappleIndicator.color = Color.green;
            }
            else
            {
                // Si hay un objeto en la capa de evitar, no cambiamos el color
                grappleIndicator.color = Color.white;
            }
        }
        else
        {
            // Si no hay objeto, volvemos a blanco
            grappleIndicator.color = Color.white;
        }
    }

    private void StartGrapple()
    {
        if (isGrappling) return;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            // Verificamos si hay un objeto en la capa de evitar entre la c�mara y el objeto grapable
            if (!Physics.Linecast(cam.position, hit.point, avoidLayer))
            {
                grapplePoint = hit.point;
                grappleIndicator.color = Color.green; // Cambiamos a verde al enganchar
            }
            else
            {
                grappleIndicator.color = Color.white; // Si hay un objeto en evitar, no cambiamos el color
                return; // No se puede enganchar
            }
        }
        else
        {
            grappleIndicator.color = Color.white; // Si no hay objeto, vuelve a blanco
            return; // No hay un punto v�lido de enganche
        }

        isGrappling = true;
        pm.freeze = false; // Deshabilitamos el freeze

        initialPosition = transform.position;
        journeyLength = Vector3.Distance(initialPosition, grapplePoint);
        startTime = Time.time;

        // Otras configuraciones que puedas necesitar
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
        StopGrapple(); // Paramos el grapple y aplicamos la f�sica

        if (rb != null)
        {
            // Calculamos la direcci�n en la que el jugador estaba yendo antes de terminar el grapple
            Vector3 grappleDirection = (grapplePoint - initialPosition).normalized;

            // Aplicamos la velocidad constante hacia adelante, usando la direcci�n hacia el punto de enganche
            Vector3 forwardMomentum = grappleDirection * postGrappleForwardVelocity;

            // A�adimos tambi�n el efecto de la gravedad para que caiga naturalmente
            rb.velocity = forwardMomentum + Vector3.down * pm.jumpForce * 0.5f;
        }
    }

    public void StopGrapple()
    {
        pm.freeze = false; // Aseguramos que el jugador no est� congelado
        isGrappling = false;

        grappleIndicator.color = Color.white; // Volvemos a blanco cuando se detiene el grapple
        reachedTarget = false;
    }

    // Nueva funci�n para detener el grapple desde fuera
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
