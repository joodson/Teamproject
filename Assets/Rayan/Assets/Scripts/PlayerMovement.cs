using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float crouchScale = 0.5f;
    [SerializeField] private float gravity = -9.81f; // gravity settings
    [SerializeField] private float jumpForce = 3f; // force applied after jumping

    [Header("Audio")]
    [SerializeField] private PlayerSoundManager footstepSound;

    // private references
    private CharacterController Controller;
    private ThirdPersonCamera _camera;

    // Movement Parameters
    private Vector3 _MoveDirection; // The Movement that will be add to the position 
    private float _horizontal;      // Horizontal Input
    private float _vertical;        // Vertical Input 

    // Jump and Gravity parametrs 
    private float _verticalVelocity; // store the virtical velocity for the gravity 

    // Crouch Settings
    [SerializeField] private float sprintScale = 1.5f;
    private bool _isCrouchToggle = false;
    bool isCrouching;
    private float _standingHeight;


    // Settings for the sounds
    private bool _wasGrounded;
    private float _footstepTimer;
    //private float _obstacleHitCooldown = 0f;


    private void Start()
    {
        // Get the Character controller
        Controller = GetComponent<CharacterController>();

        // Getting the Y scale
        _standingHeight = Controller.height;

        // Get the camera script
        _camera = Camera.main.GetComponent<ThirdPersonCamera>();
    }

    private void Update()
    {
        bool groundedThisFrame = Controller.isGrounded;

        /*
        if (_obstacleHitCooldown > 0f)
        {
            _obstacleHitCooldown -= Time.deltaTime;
        }
        */

        ApplyGravity();
        HandleCrouch();
        HandleJump();
        HandleMovement();
        HandleFootsteps();
        HandleLanding(groundedThisFrame);

        _wasGrounded = groundedThisFrame;
    }

    // Movement method
    private void HandleMovement()
    {
        // Input from the player
        _horizontal = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right arrows
        _vertical = Input.GetAxisRaw("Vertical");     // W/S or Up/Down arrows

        // Get the camera forward and right 
        Vector3 forward = _camera.GetCameraForward();
        Vector3 right = _camera.GetCameraRight();

        if (_camera.IsAiming())
        {
            // Player is aiming
            walkSpeed = 2f;
        }
        else
        {
            walkSpeed = 5f;
        }

        // calculate the movement 
        _MoveDirection = (forward * _vertical + right * _horizontal).normalized * walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _MoveDirection *= sprintScale;
        }

        // Rotate player to face movement direction
        if (_MoveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(_MoveDirection.x, 0f, _MoveDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Taking the velocity and apply it to the move direction
        _MoveDirection.y = _verticalVelocity;

        // Moving the character 
        Controller.Move(_MoveDirection * Time.deltaTime);
    }

    // footsteps 
    private void HandleFootsteps()
    {
        if (_MoveDirection.magnitude > 0.1f && Controller.isGrounded)
        {
            MovementState state = GetCurrentMovementState();
            PlayerSoundManager.Instance.PlayFootstep(state, Time.deltaTime);
        }
        else
        {
            PlayerSoundManager.Instance.ResetFootstepTimer();
        }
    }

    private void HandleCrouch()
    {
        // Check for toggle (C key)
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouchToggle = !_isCrouchToggle;
        }

        // Check for hold (Left Control) or the C button pressed

        isCrouching = _isCrouchToggle || Input.GetKey(KeyCode.LeftControl);

        // Set Controller.height based on crouching
        // Set scale based on crouching
        if (isCrouching)
        {
            Controller.height = _standingHeight * 0.5f; // Half height
        }
        else
        {
            Controller.height = _standingHeight; // Full height
        }

        // Apply crouchScale to movement if crouching
    }

    private void ApplyGravity()
    {
        // checking if the character is grounded 
        if (Controller.isGrounded && _verticalVelocity < 0) // isGrounded is built in check
        {
            _verticalVelocity = -2f; // Reset the vertical velocity (gravity)
        }
        else
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }

    }

    private void HandleJump()
    {
        if ( Input.GetButtonDown("Jump") && Controller.isGrounded && !isCrouching)
        {
            _verticalVelocity = jumpForce;
            PlayerSoundManager.Instance.PlayJumpSound();
        }
    }


    private void HandleLanding(bool groundedThisFrame)
    {
        if (!_wasGrounded && groundedThisFrame)
        {
            PlayerSoundManager.Instance.PlayLandSound();
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_MoveDirection.magnitude > 0.1f)        //if (_MoveDirection.magnitude > 0.1f && _obstacleHitCooldown <= 0f)
        {
            PlayerSoundManager.Instance.PlayObstacleSound();
            //_obstacleHitCooldown = 0.5f; // Can't play again for 0.5 seconds
        }
    }

    private MovementState GetCurrentMovementState()
    {
        if (_isCrouchToggle || Input.GetKey(KeyCode.LeftControl))
            return MovementState.Crouching;

        if (Input.GetKey(KeyCode.LeftShift))
            return MovementState.Running;

        return MovementState.Walking;
    }

}