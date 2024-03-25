using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public float walkSpeed = 6f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 3f;
    public float jumpForce = 8f;
    public float jumpCooldown = 0.5f;
    public float airMultiplier = 0.5f;
    public float crouchYScale = 0.5f;
    public float groundDrag = 6f;
    public float playerHeight = 2f;
    public float maxSlopeAngle = 45f;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    public LayerMask whatIsGround;

    public float MoveSpeed { get; set; }
    public float StartYScale { get; private set; }

    private void Start()
    {
        MoveSpeed = walkSpeed;
        StartYScale = transform.localScale.y;
    }
}