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

    public WallRunningAdvanced wallRunning;
    public bool CantGrapple = false;

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

        wallRunning = GetComponent<WallRunningAdvanced>();

        targetColor = Color.cyan; // Color inicial
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey) && CantGrapple ==false)
        {
            StartGrapple();
        }


        // Manejo del cooldown sin interferir con el movimiento mientras está grappling
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

        CheckForGrappleable();
    }

    private void CheckForGrappleable()
    {
        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, maxGrappleDistance);

        // Variables para controlar si hemos golpeado algo en cada capa
        RaycastHit? grappleHit = null;
        RaycastHit? avoidHit = null;

        // Iterar sobre los resultados de todos los raycasts
        foreach (RaycastHit hit in hits)
        {
            // Comprobar si el objeto pertenece a la capa de objetos grappleables
            if (((1 << hit.collider.gameObject.layer) & whatIsGrappleable) != 0)
            {
                grappleHit = hit; // Si encontramos un objeto grappleable, lo almacenamos

                if (wallRunning.isWallrunning == true)
                {
                    CantGrapple = false;
                }
            }
            // Comprobar si el objeto pertenece a las capas que queremos evitar
            else if (((1 << hit.collider.gameObject.layer) & avoidLayers) != 0)
            {
                avoidHit = hit; // Si encontramos un objeto en avoidLayers, lo almacenamos

                if (wallRunning.isWallrunning == true)
                {
                    CantGrapple = true;
                }
                else
                {
                    CantGrapple = false;
                }
            }
        }

        // Si existe un objeto en avoidLayers y está más cerca que el objeto grappleable, lo bloqueamos
        if (avoidHit.HasValue && (!grappleHit.HasValue || avoidHit.Value.distance < grappleHit.Value.distance))
        {
            // Si hay un objeto en avoidLayers que bloquea la visión, ponemos la mira en blanco
            crosshair.color = Color.white;
        }
        else if (grappleHit.HasValue)
        {
            // Si encontramos un objeto grappleable y no está bloqueado, ponemos la mira en verde
            crosshair.color = Color.green;
        }
        else
        {
            // Si no encontramos nada grappleable o bloqueado, mantenemos la mira en blanco
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
        // Permitir grappling si el cooldown está en 0, o si el jugador está en modo wallrunning
        if ((grapplingCdTimer > 0 || grappling) && !wallRunning.pm.wallrunning) return;

        grappling = true;
        audioM.PlaySfx(2);

        pm.freeze = true;

        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, maxGrappleDistance);

        RaycastHit? grappleHit = null;
        RaycastHit? avoidHit = null;

        // Iterar sobre los resultados de todos los raycasts
        foreach (RaycastHit hit in hits)
        {
            // Comprobar si el objeto pertenece a la capa de objetos grappleables
            if (((1 << hit.collider.gameObject.layer) & whatIsGrappleable) != 0)
            {
                grappleHit = hit; // Si encontramos un objeto grappleable, lo almacenamos
            }
            // Comprobar si el objeto pertenece a las capas que queremos evitar
            else if (((1 << hit.collider.gameObject.layer) & avoidLayers) != 0)
            {
                avoidHit = hit; // Si encontramos un objeto en avoidLayers, lo almacenamos
            }
        }

        // Si existe un objeto en avoidLayers y está más cerca que el objeto grappleable, bloqueamos el grappling
        if (avoidHit.HasValue && (!grappleHit.HasValue || avoidHit.Value.distance < grappleHit.Value.distance))
        {
            Debug.Log("Gancho bloqueado por objeto en capas evitadas.");
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        else if (grappleHit.HasValue)
        {
            // Si encontramos un objeto grappleable, ejecutamos el grappling
            grapplePoint = grappleHit.Value.point;

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

        // Aquí activamos StopGrapple pero **NO** activamos el cooldown inmediatamente
        // Permitimos que el jugador complete el viaje antes de iniciar el cooldown
        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        pm.freeze = false;
        grappling = false;

        // Una vez que el grappling ha terminado, **ahora** activamos el cooldown
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