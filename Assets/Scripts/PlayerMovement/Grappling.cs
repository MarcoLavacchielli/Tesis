using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esto para trabajar con UI

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
        grappleIndicator.color = Color.white; // Asegúrate de que el color inicial sea blanco

        rb.interpolation = RigidbodyInterpolation.Interpolate;
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
            // Verificamos si hay un objeto en la capa de evitar entre la cámara y el objeto grapable
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
            // Verificamos si hay un objeto en la capa de evitar entre la cámara y el objeto grapable
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
            return; // No hay un punto válido de enganche
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

    [Header("Post Grapple Settings")]
    public float upwardImpulse = 3f; // Ajusta el valor según lo que necesites


    private void ApplyGrappleCompletion()
    {
        pm.freeze = false; // Aseguramos que el jugador no esté congelado
        isGrappling = false;
        StopGrappleUnique(); // Paramos el grapple y aplicamos la física

        if (rb != null)
        {
            // Calculamos la dirección en la que el jugador estaba yendo antes de terminar el grapple
            Vector3 grappleDirection = (grapplePoint - initialPosition).normalized;

            // Aplica un impulso constante hacia adelante en la dirección del grapple
            Vector3 forwardMomentum = grappleDirection * postGrappleForwardVelocity;

            // Siempre aplicamos el mismo impulso hacia arriba
            Vector3 upwardMomentum = Vector3.up * upwardImpulse; // Ajuste de variable 

            // Asegurarse de que se aplica el mismo impulso sin importar las circunstancias
            rb.velocity = forwardMomentum + upwardMomentum; // Usa un valor constante aquí
        }
    }


    public void StopGrapple()
    {
        pm.freeze = false; // Aseguramos que el jugador no esté congelado
        isGrappling = false;

        grappleIndicator.color = Color.white; // Volvemos a blanco cuando se detiene el grapple
        reachedTarget = false;
    }

    public void StopGrappleUnique()
    {
        grappleIndicator.color = Color.white; // Volvemos a blanco cuando se detiene el grapple
        reachedTarget = false;
    }

    // Nueva función para detener el grapple desde fuera
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
