using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para manipular UI

public class Grappling : MonoBehaviour
{
    AudioManager audioM;

    [Header("References")]
    private PlayerMovementGrappling pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LayerMask avoidLayers;
    public LineRenderer lr;
    public Material LuzGancho;

    public Image crosshair; // Referencia a la imagen de la mira en el Canvas

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;

    private Vector3 grapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;

    private bool grappling;
    private Color targetColor;
    private float colorTransitionSpeed = 2.0f;
    private bool isCoolingDown;

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
        pm = GetComponent<PlayerMovementGrappling>();
        targetColor = Color.cyan; // Color inicial
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey)) StartGrapple();

        // Manejo del cooldown
        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
            targetColor = Color.red;
            isCoolingDown = true;
        }
        else
        {
            if (isCoolingDown)
            {
                targetColor = Color.cyan;
                isCoolingDown = false;
            }
        }

        if (isCoolingDown)
        {
            LuzGancho.SetColor("_Color", Color.red);
        }
        else
        {
            LuzGancho.SetColor("_Color", Color.Lerp(LuzGancho.GetColor("_Color"), targetColor, Time.deltaTime * colorTransitionSpeed));
        }

        // Comprobar si la mira está sobre un objeto grapleable
        CheckForGrappleable();
    }

    private void CheckForGrappleable()
    {
        // Verificar primero si golpeamos algo en las capas de avoidLayers
        RaycastHit avoidHit;
        if (Physics.Raycast(cam.position, cam.forward, out avoidHit, maxGrappleDistance, avoidLayers))
        {
            // Si golpeamos algo en avoidLayers, cambiamos la mira a blanco
            crosshair.color = Color.white;
            return;
        }

        // Si no golpeamos nada en avoidLayers, verificamos los objetos grappleables
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            // Si golpea un objeto grappleable, cambiamos el color a verde
            crosshair.color = Color.green;
        }
        else
        {
            // Si no golpea nada grappleable, cambiamos la mira a blanco
            crosshair.color = Color.white;
        }
    }

    private void LateUpdate()
    {
        // if (grappling)
        //    lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0 || grappling) return;

        grappling = true;
        audioM.PlaySfx(2);

        pm.freeze = true;

        // Verificar primero si golpeamos algo en las capas de avoidLayers
        RaycastHit avoidHit;
        if (Physics.Raycast(cam.position, cam.forward, out avoidHit, maxGrappleDistance, avoidLayers))
        {
            // Si golpea un objeto en avoidLayers, cancelamos el grapple
            Debug.Log("Gancho bloqueado por objeto en capas evitadas.");
            Invoke(nameof(StopGrapple), grappleDelayTime);
            return; // Cancelamos el proceso de grappling
        }

        // Si no golpeamos nada en avoidLayers, procedemos a comprobar los objetos grappleables
        RaycastHit grappleHit;
        if (Physics.Raycast(cam.position, cam.forward, out grappleHit, maxGrappleDistance, whatIsGrappleable))
        {
            // Si golpeamos un objeto grappleable, procedemos con el grappling
            grapplePoint = grappleHit.point;

            // Activar cooldown
            grapplingCdTimer = grapplingCd;

            // Ejecutar el grappling
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            // Si no golpeamos nada grappleable, el punto final es en línea recta
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        audioM.PlaySfx(6);

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;

        grappling = false;

        if (grapplingCdTimer <= 0)
        {
            grapplingCdTimer = grapplingCd;
        }
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
