using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private WallRunningAdvanced wallRunning;

    public Image crosshair; // Puntito del canvas

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
    private Color targetColor; // En caso de poner un arma o material, esta es la textura que cambia de color
    private float colorTransitionSpeed = 2.0f;
    private bool isCoolingDown;

    [SerializeField] private bool hasTouchedGroundOrWall; // Booleano para ver si funciona el render (quedo medio irrele a este punto)

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
        targetColor = Color.cyan;
    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey) && CanGrapple())
        {
            StartGrapple();
        }

        // Cooldown, junto con el cambio de material
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

    private bool CanGrapple()
    {
        bool isWallRunning = CheckWallRunning();

        if (isWallRunning)
        {
            return false;
        }

        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, maxGrappleDistance);
        RaycastHit? grappleHit = null;
        RaycastHit? avoidHit = null;

        // Para que el gancho solo se use en ganchos, que solo pase analice layers de whatisgrappable
        foreach (RaycastHit hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & whatIsGrappleable) != 0)
            {
                grappleHit = hit;
            }
            else if (((1 << hit.collider.gameObject.layer) & avoidLayers) != 0)
            {
                avoidHit = hit;
            }
        }

        if (avoidHit.HasValue && (!grappleHit.HasValue || avoidHit.Value.distance < grappleHit.Value.distance))
        {
            return false;
        }

        if (grappleHit.HasValue)
        {
            return true;
        }

        return false;
    }


    private bool CheckWallRunning()
    {
        WallRunningAdvanced wallRunningScript = GetComponent<WallRunningAdvanced>();
        return wallRunningScript != null && wallRunningScript.wallrunning;
    }

    private void CheckForGrappleable()
    {
        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, maxGrappleDistance);

        RaycastHit? grappleHit = null;
        RaycastHit? avoidHit = null;

        foreach (RaycastHit hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & whatIsGrappleable) != 0)
            {
                grappleHit = hit;
            }
            else if (((1 << hit.collider.gameObject.layer) & avoidLayers) != 0)
            {
                avoidHit = hit;
            }
        }

        if (avoidHit.HasValue && (!grappleHit.HasValue || avoidHit.Value.distance < grappleHit.Value.distance))
        {
            crosshair.color = Color.white;
        }
        else if (grappleHit.HasValue)
        {
            crosshair.color = Color.green;
        }
        else
        {
            crosshair.color = Color.white;
        }
    }

    private void StartGrapple()
    {
        Debug.Log("FLAG EMPIEZA GANCHO");

        if (grapplingCdTimer > 0 || grappling) return;

        grappling = true;
        audioM.PlaySfx(2);
        pm.freeze = true;
        hasTouchedGroundOrWall = false; // Reseteo el bool, no toco

        RaycastHit[] hits = Physics.RaycastAll(cam.position, cam.forward, maxGrappleDistance);

        RaycastHit? grappleHit = null;
        RaycastHit? avoidHit = null;

        foreach (RaycastHit hit in hits)
        {
            if (((1 << hit.collider.gameObject.layer) & whatIsGrappleable) != 0)
            {
                grappleHit = hit;
            }
            else if (((1 << hit.collider.gameObject.layer) & avoidLayers) != 0)
            {
                avoidHit = hit;
            }
        }

        if (avoidHit.HasValue && (!grappleHit.HasValue || avoidHit.Value.distance < grappleHit.Value.distance))
        {
            Debug.Log("Gancho bloqueado por objeto en capas evitadas.");
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
        else if (grappleHit.HasValue)
        {
            grapplePoint = grappleHit.Value.point;
            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }
    }

    private void ExecuteGrapple()
    {
        Debug.Log("EJECUTA EL VIAJE");

        pm.freeze = false;
        audioM.PlaySfx(6);

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);
    }

    public void StopGrapple()
    {
        Debug.Log("EL GANCHO TERMINA");

        if (hasTouchedGroundOrWall)
        {
            pm.freeze = false;
            grappling = false;

            if (grapplingCdTimer <= 0)
            {
                grapplingCdTimer = grapplingCd;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & avoidLayers) != 0)
        {
            hasTouchedGroundOrWall = true;
            StopGrapple();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Se stopea el gancho
        if (((1 << other.gameObject.layer) & avoidLayers) != 0)
        {
            hasTouchedGroundOrWall = true;
            StopGrapple();
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
