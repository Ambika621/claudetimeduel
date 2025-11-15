
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float sprintMultiplier = 1.5f;
    public float acceleration = 10f;
    public float airControl = 0.5f;
    
    [Header("Jump & Dash")]
    public float jumpForce = 12f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;
    
    private Rigidbody rb;
    private Vector3 velocity;
    private Vector3 moveInput;
    private bool isGrounded;
    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        // Input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector3(horizontal, 0, vertical).normalized;
        
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
        
        // Dash
        dashCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartDash();
        }
        
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                isDashing = false;
            }
        }
    }
    
    void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = transform.forward * dashSpeed;
            return;
        }
        
        // Movement with acceleration
        float currentSpeed = Input.GetKey(KeyCode.LeftControl) ? moveSpeed * sprintMultiplier : moveSpeed;
        float controlFactor = isGrounded ? 1f : airControl;
        
        Vector3 targetVelocity = moveInput * currentSpeed;
        Vector3 velocityChange = (targetVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z)) * controlFactor;
        
        rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
        
        // Rotation towards movement
        if (moveInput.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 15f);
        }
    }
    
    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
    }
    
    public Vector3 GetMoveInput() => moveInput;
    public bool IsDashing() => isDashing;
}
