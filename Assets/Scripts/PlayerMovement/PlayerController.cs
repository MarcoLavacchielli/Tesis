using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerModel playerModel;
    private Rigidbody rb;
    private Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    private bool grounded;
    private bool readyToJump;
    private bool exitingSlope;

    private void Start()
    {
        playerModel = GetComponent<PlayerModel>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        orientation = transform.Find("Orientation");
        if (orientation == null)
            Debug.LogError("No orientation transform found. Make sure to create an empty GameObject named 'Orientation' as a child of the player.");

        readyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerModel.playerHeight * 0.5f + 0.2f, playerModel.whatIsGround);

        MyInput();
        StateHandler();

        if (grounded)
            rb.drag = playerModel.groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(playerModel.jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), playerModel.jumpCooldown);
        }

        if (Input.GetKeyDown(playerModel.crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, playerModel.crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        if (Input.GetKeyUp(playerModel.crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, playerModel.StartYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        if (Input.GetKey(playerModel.crouchKey))
            playerModel.MoveSpeed = playerModel.crouchSpeed;
        else if (grounded && Input.GetKey(playerModel.sprintKey))
            playerModel.MoveSpeed = playerModel.sprintSpeed;
        else if (grounded)
            playerModel.MoveSpeed = playerModel.walkSpeed;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * playerModel.MoveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (grounded)
            rb.AddForce(moveDirection.normalized * playerModel.MoveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * playerModel.MoveSpeed * 10f * playerModel.airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * playerModel.jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    private bool OnSlope()
    {
        RaycastHit slopeHit;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerModel.playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < playerModel.maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        RaycastHit slopeHit;
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerModel.playerHeight * 0.5f + 0.3f))
            return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
        return Vector3.zero;
    }
}