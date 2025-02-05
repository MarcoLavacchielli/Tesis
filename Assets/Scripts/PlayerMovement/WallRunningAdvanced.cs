using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunningAdvanced : MonoBehaviour
{

    public AudioManager audioM;

    public bool wallrunningFlagSound = false;

    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    public bool wallrunning { get; private set; }

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    public Transform playercamToTilt; // Nueva referencia para la cámara a inclinar
    public float tiltAngle = 10f; // Ángulo de inclinación de la cámara
    public float tiltSpeed = 2f; // Velocidad de inclinación
    public PlayerMovementAdvanced pm;
    private LedgeGrabbing lg;
    private Rigidbody rb;
    private float currentTilt = 0f; // Para almacenar el ángulo actual de inclinación

    //
    private PlayerMovementGrappling pg;
    //

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovementAdvanced>();
        lg = GetComponent<LedgeGrabbing>();

        pg = GetComponent<PlayerMovementGrappling>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();

        if (Input.GetKeyDown(upwardsRunKey))
        {
            upwardsRunning = true;
        }
        if (Input.GetKeyUp(upwardsRunKey))
        {
            upwardsRunning = false;
        }
        if (Input.GetKeyDown(downwardsRunKey))
        {
            downwardsRunning = true;
        }
        if (Input.GetKeyUp(downwardsRunKey))
        {
            downwardsRunning = false;
        }
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();

        HandleCameraTilt(); // Aplicar el tilt en cada FixedUpdate
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {

        // Si el grappling está activo, salir del modo wallrunning
        if (GetComponent<Grappling>().IsGrappling())
        {
            if (pm.wallrunning)
                StopWallRun();
            return;
        }

        // Getting Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // State 1 - Wallrunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pm.wallrunning)
                StartWallRun();

            // wallrun timer
            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0 && pm.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }

            // wall jump
            if (Input.GetKeyDown(jumpKey)) WallJump();
        }

        // State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        // State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // apply camera effects
        if (cam == null) return;
        cam.DoFov(90f);

        pg.moveSpeed = 6;
        wallrunningFlagSound = true;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // upwards/downwards force
        if (upwardsRunning == true)
        {
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        }
        else if (upwardsRunning == false)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        // push to wall force
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        // weaken gravity
        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;

        // reset camera effects
        if (cam == null) return;
        cam.DoFov(80f);

        wallrunningFlagSound = false;
        audioM.PauseSFX(13);
    }

    private void WallJump()
    {
        if (lg.holding || lg.exitingLedge) return;

        // enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);

        audioM.PlaySfx(3);
    }

    private void HandleCameraTilt()
    {
        float targetTilt = 0f;

        if (pm.wallrunning)
        {
            if (wallLeft)
                targetTilt = -tiltAngle;
            else if (wallRight)
                targetTilt = tiltAngle;
        }

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        playercamToTilt.localRotation = Quaternion.Euler(0, 0, currentTilt);
    }
}
