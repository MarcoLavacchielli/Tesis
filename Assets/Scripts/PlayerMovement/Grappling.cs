using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    AudioManager audioM;

    [Header("References")]
    private PlayerMovementGrappling pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    public Material LuzGancho; // Añadir referencia al material

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
    private float colorTransitionSpeed = 2.0f; // Ajusta la velocidad de transición del color
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

        if (grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
            targetColor = Color.red; // Cambiar objetivo a rojo cuando está en cooldown
            isCoolingDown = true;
        }
        else
        {
            if (isCoolingDown)
            {
                targetColor = Color.cyan; // Cambiar objetivo a cian cuando no está en cooldown
                isCoolingDown = false;
            }
        }

        if (isCoolingDown)
        {
            // Cambiar bruscamente a rojo cuando está en cooldown
            LuzGancho.SetColor("_Color", Color.red);
        }
        else
        {
            // Transición suave de rojo a cian
            LuzGancho.SetColor("_Color", Color.Lerp(LuzGancho.GetColor("_Color"), targetColor, Time.deltaTime * colorTransitionSpeed));
        }
    }

    private void LateUpdate()
    {
        // if (grappling)
        //    lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;

        grappling = true;
        audioM.PlaySfx(2);

        pm.freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            //audioM.PlaySfx(6);

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        //lr.enabled = true;
        //lr.SetPosition(1, grapplePoint);
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

        grapplingCdTimer = grapplingCd;

        //lr.enabled = false;
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
