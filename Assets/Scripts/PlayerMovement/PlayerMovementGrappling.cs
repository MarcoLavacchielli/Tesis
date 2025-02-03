using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementGrappling : MonoBehaviour
{
    AudioManager audioM;

    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float swingSpeed;

    public float groundDrag;


    [Header("Shader Control")]
    public Material PostProcessRun;
    private float shaderValue;
    private Coroutine shaderCoroutine;
    private bool isShaderTransitioning = false;

    [Header("Jumping")]
    public float jumpForce;
    public float coyoteJumpForce;  // Fuerza del salto durante el Coyote Time
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Camera Effects")]
    public PlayerCam cam;
    public float grappleFov = 95f;

    [Header("Coyote Time")]
    public float coyoteTimeDuration = 0.5f;
    private float coyoteTimeCounter;
    [SerializeField] private bool hasJumped;
    private bool isCoyoteJump;  // Nueva variable para detectar si es un salto coyote

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    public Rigidbody rb;

    WallRunningAdvanced pwl;

    private bool wasInAir;

    public MovementState state;
    public enum MovementState
    {
        freeze,
        grappling,
        swinging,
        walking,
        sprinting,
        crouching,
        air
    }

    public bool freeze;
    public bool activeGrapple;
    public bool swinging;

    private void Awake()
    {
        audioM = FindObjectOfType<AudioManager>();

        pwl = GetComponent<WallRunningAdvanced>();

        if (audioM == null)
        {
            Debug.LogError("No AudioManager found in the scene!");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;
        startYScale = transform.localScale.y;

        if (PostProcessRun != null)
        {
            PostProcessRun.SetFloat("_IsActive", 1f);
            shaderValue = 1f;
        }
    }

    private void Update()
    {
        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // Reproducir SFX cuando aterrice
        if (grounded && wasInAir)
        {
            audioM.PlaySfx(14); // Reproducir sonido de aterrizaje
            wasInAir = false;  // Reiniciar estado al tocar el suelo
        }

        if (!grounded)
        {
            wasInAir = true; // Marcar que el jugador está en el aire
        }

        // Reset coyote time and hasJumped when grounded
        if (grounded)
        {
            coyoteTimeCounter = coyoteTimeDuration;
            hasJumped = false;
        }
        else if (!grounded && coyoteTimeCounter > 0 && !hasJumped)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        else if (coyoteTimeCounter <= 0)
        {
            hasJumped = true;
        }

        MyInput();
        SpeedControl();
        StateHandler();

        // Handle drag
        if (grounded && !activeGrapple)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        // Movement input using only WASD
        horizontalInput = 0f;
        verticalInput = 0f;

        if (Input.GetKey(KeyCode.W)) verticalInput += 1f;
        if (Input.GetKey(KeyCode.S)) verticalInput -= 1f;
        if (Input.GetKey(KeyCode.A)) horizontalInput -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput += 1f;

        // Jumping logic
        if (Input.GetKey(jumpKey) && readyToJump && (grounded || (coyoteTimeCounter > 0 && !hasJumped)))
        {
            readyToJump = false;
            hasJumped = true;

            // Verificar si es un salto coyote
            isCoyoteJump = !grounded && coyoteTimeCounter > 0;

            Jump();  // Llama al método Jump()
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Crouch logic
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }


    private void StateHandler()
    {
        if (freeze)
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
            audioM.PauseSFX(13);
            ChangeShaderValue(1f);
        }
        else if (activeGrapple)
        {
            state = MovementState.grappling;
            moveSpeed = sprintSpeed;
            audioM.PauseSFX(13);
            ChangeShaderValue(1f);
        }
        else if (swinging)
        {
            state = MovementState.swinging;
            moveSpeed = swingSpeed;
            audioM.PauseSFX(13);
            ChangeShaderValue(1f);
        }
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            PlayWalkSound();
            ChangeShaderValue(1f);
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            PlayWalkSound();
            Debug.Log("DEBERIA BAJAR");

            // Cambiar el valor del shader solo si no está en transición
            if (!isShaderTransitioning)
            {
                ChangeShaderValue(0f); // Inicia la transición a 0
                isShaderTransitioning = true; // Marca que la transición ha comenzado
            }
        }
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            PlayWalkSound();

            // Si el estado ya no es sprinting, cambiar el valor del shader a 1
            if (state != MovementState.sprinting && isShaderTransitioning)
            {
                ChangeShaderValue(1f); // Restablecer el valor del shader a 1
                isShaderTransitioning = false; // Permitir que la transición se ejecute nuevamente en el futuro
            }
        }
        else
        {
            state = MovementState.air;
            if (pwl.wallrunningFlagSound)
            {
                PlayWalkSound();
            }
            else
            {
                audioM.PauseSFX(13);
            }

            // Si el estado no es sprinting, poner el shader en 1
            if (state != MovementState.sprinting && isShaderTransitioning)
            {
                ChangeShaderValue(1f); // Restablecer el valor del shader a 1
                isShaderTransitioning = false; // Permitir que la transición se ejecute nuevamente en el futuro
            }

            ChangeShaderValue(1f);
        }
    }


    private void PlayWalkSound()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (flatVelocity.magnitude > 0.1f)
        {
            if (!audioM.sfxSource[13].isPlaying)
            {
                audioM.PlaySFXLoop(13);  // Reproducir el sonido de caminar en loop
            }

            // Cambiar pitch y volumen según el estado
            if (state == MovementState.sprinting)
            {
                audioM.sfxSource[13].pitch = 2.0f;   // Doble velocidad para sprint
                //audioM.sfxSource[13].volume = 1.5f; // Volumen normal para sprint
            }

            else if (state == MovementState.crouching)
            {
                audioM.sfxSource[13].pitch = 1.2f;   // Mitad de velocidad para crouching
                //audioM.sfxSource[13].volume = 0.3f;  // Volumen reducido para crouching
            }

            else if (state == MovementState.air)
            {
                audioM.sfxSource[13].pitch = 2f;   // Mitad de velocidad para crouching
                //audioM.sfxSource[13].volume = 0.3f;  // Volumen reducido para crouching
            }
            else
            {
                audioM.sfxSource[13].pitch = 1.5f;   // Velocidad normal para caminar
                //audioM.sfxSource[13].volume = 0.55f;  // Volumen normal para caminar
            }
        }
        else
        {
            audioM.PauseSFX(13);  // Pausar el sonido si está quieto
        }
    }


    private void MovePlayer()
    {
        if (activeGrapple || swinging) return;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (activeGrapple) return;

        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {

        exitingSlope = true;

        // Reiniciar la velocidad vertical para evitar acumulación de velocidad
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // Usar una fuerza diferente dependiendo de si es un salto coyote o un salto normal
        if (isCoyoteJump)
        {
            rb.AddForce(transform.up * coyoteJumpForce, ForceMode.Impulse);  // Usar la fuerza del coyote
        }
        else
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);  // Usar la fuerza normal
        }

        audioM.PauseSFX(13);
        audioM.PlaySfx(3);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
        isCoyoteJump = false;  // Resetear la variable
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;

        cam.DoFov(grappleFov);
    }

    public void ResetRestrictions()
    {
        activeGrapple = false;
        cam.DoFov(85f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    private void ResetShaderTransition()
    {
        isShaderTransitioning = false; // Permite que la transición vuelva a ocurrir
    }

    private void ChangeShaderValue(float targetValue)
    {
        if (shaderCoroutine != null)
        {
            StopCoroutine(shaderCoroutine);
        }
        shaderCoroutine = StartCoroutine(LerpShaderValue(targetValue));
    }

    private IEnumerator LerpShaderValue(float targetValue)
    {
        float time = 0f;
        float duration = 0.5f; // Duración de la transición
        float startValue = shaderValue;

        targetValue = Mathf.Clamp01(targetValue);

        while (time < duration)
        {
            shaderValue = Mathf.Lerp(startValue, targetValue, time / duration);
            PostProcessRun.SetFloat("_IsActive", shaderValue);
            shaderValue = Mathf.Clamp01(shaderValue);
            time += Time.deltaTime;
            yield return null;
        }

        shaderValue = targetValue;
        PostProcessRun.SetFloat("_IsActive", shaderValue);

        // Resetear el flag una vez que la transición ha terminado
        if (targetValue == 1f)
        {
            ResetShaderTransition(); // Permite que se ejecute la transición nuevamente
        }
    }

}
