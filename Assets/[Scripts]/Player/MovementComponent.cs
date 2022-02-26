using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementComponent : MonoBehaviour
{
    // Components
    private PlayerController _playerController;
    private Rigidbody m_rb;
    private Animator _playerAnimator;

    // Movement
    [Header("Movement")]
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float runSpeed = 10f;
    [SerializeField]
    private float jumpForce = 5f;

    // Look
    private Vector2 lookInput = Vector2.zero;
    public float aimSensitivity = 0.5f;
    public GameObject followTarget;

    // Animations
    [Header("Animations")]
    public readonly int movementXHash = Animator.StringToHash("MovementX");
    public readonly int movementYHash = Animator.StringToHash("MovementY");
    public readonly int isJumpingHash = Animator.StringToHash("IsJumping");
    public readonly int isRunningHash = Animator.StringToHash("IsRunning");

    // References


    // Executions
    private Vector2 inputVector = Vector2.zero;
    private Vector3 moveDirection = Vector3.zero;

    private void Awake()
    {
        // Get all required components
        _playerController = GetComponent<PlayerController>();
        _playerAnimator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If the input vector catches some input, the there is movement to the character, else set direction to 0
        if (!(inputVector.magnitude > 0))
        {
            moveDirection = Vector3.zero;
        }

        // Set direction using the inputVector and respective forward and right vectors
        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;

        // Set current speed based on running or not
        float currentSpeed = _playerController.isRunning ? runSpeed : walkSpeed;

        // Position update vector with current Speed
        Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);
        transform.position += movementDirection;
    }

    //-------------------------------------- Movement Functions --------------------------------------//

    /// <summary>
    /// On Movement function for the movement action
    /// from the new Input System
    /// </summary>
    /// <param name="value"></param>
    public void OnMovement(InputValue value)
    {
        // Movement vector
        inputVector = value.Get<Vector2>();
    }


    /// <summary>
    /// On Jump function for the jump action
    /// from the new Input System
    /// </summary>
    /// <param name="value"></param>
    public void OnJump(InputValue value)
    {
        if (_playerController.isJumping)
        {
            return;
        }

        // Jump bool
        _playerController.isJumping = true;

        // Add force
        m_rb.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// On Run function for the Run action from the new Input System
    /// </summary>
    /// <param name="value"></param>
    public void OnRun(InputValue value)
    {
        // if in mid air already, cannot increase speed
        if (_playerController.isJumping)
        {
            return;
        }

        // set player controller is running check to true
        _playerController.isRunning = value.isPressed;
    }


    /// <summary>
    /// A Use function to interact if necessary
    /// </summary>
    /// <param name="value"></param>
    public void OnUse(InputValue value)
    {
        if (_playerController.isUsing)
        {
            return;
        }

        _playerController.isUsing = true;
    }


    //-------------------------------------- Collision Functions --------------------------------------//

    /// <summary>
    /// On Collision Enter for the capsule collider
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (!_playerController.isJumping)
            {
                return;
            }

            // Colliding with ground means not jumping
            _playerController.isJumping = false;
        }
    }
}
