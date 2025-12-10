using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target; // The player
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -5f); // Camera offset from target

    [Header("Camera Settings")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float smoothSpeed = 10f; // How smoothly camera follows

    [Header("Rotation Limits")] // Prevents camera from flipping upside down  Basicly useless code
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 90f;

    [Header("Aim/Zoom Settings")]
    [SerializeField] private Vector3 aimOffset = new Vector3(1f, 1.5f, -3f);
    [SerializeField] private float aimFOV = 40f;
    [SerializeField] private float normalFOV = 60f;
    [SerializeField] private float aimTransitionSpeed = 10f;

    private bool _isAiming = false;
    private Vector3 _currentOffset;
    private Camera _camera;

    // Private variables
    private float _currentX = 0f; // Horizontal rotation
    private float _currentY = 20f; // Vertical rotation (start slightly above)


    private void Start()
    {
        // Lock and hide cursor for better gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        target = target.transform;

        _camera = GetComponent<Camera>();
        _currentOffset = offset;

        _camera.fieldOfView = normalFOV;
    }

    private void LateUpdate()
    {
        HandleAimInput();  //checks if right-click is pressed
        HandleRotation();
        HandlePosition();
        HandleZoom();      //handles FOV changes


        // Press ESC to unlock cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void HandleRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Update rotation angles
        _currentX += mouseX;
        _currentY -= mouseY; // Inverted for natural feel

        // Clamp vertical rotation
        _currentY = Mathf.Clamp(_currentY, minVerticalAngle, maxVerticalAngle);
    }

    private void HandlePosition()
    {
        // Calculate desired rotation
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0f);


        // checking if the player is aiming or not and adjust the FOV to the disierd one
        Vector3 targetOffset = _isAiming ? aimOffset : offset;

        // changing the cerrent camera postion based on if the player hold the zoom button or not
        _currentOffset = Vector3.Lerp(_currentOffset, targetOffset, aimTransitionSpeed * Time.deltaTime);

        // Calculate desired position
        Vector3 desiredPosition = target.position + rotation * _currentOffset;


        // Smoothly move camera to position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        // Always look infront of the player
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothSpeed * Time.deltaTime);
    }

    // Public method to get camera's forward direction (useful for player movement)
    public Vector3 GetCameraForward()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f; // Remove vertical component
        return forward.normalized;
    }

    public Vector3 GetCameraRight()
    {
        Vector3 right = transform.right;
        right.y = 0f;
        return right.normalized;
    }

    private void HandleZoom()
    {
        if (_camera == null)
            return;

        float targetFOV = _isAiming ? aimFOV : normalFOV;
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, targetFOV, aimTransitionSpeed * Time.deltaTime);
    }

    private void HandleAimInput()
    {
        _isAiming = Input.GetButton("Fire2"); // Fire2 = Right mouse button
    }

    public bool IsAiming()
    {
        return _isAiming;
    }
}