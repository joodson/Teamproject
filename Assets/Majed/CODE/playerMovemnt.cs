using UnityEngine;

public class playerMovemnt : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float rotationSpeed = 10f;

    [Header("Jump")]
    public float jumpForce = 8f;
    public float doubleJumpForce = 8f;

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Camera Offset")]
    public float cameraHeight = 3f;

    private CharacterController controller;
    private Vector3 moveDirection;
    private float gravityVelocity;
    private bool canDoubleJump = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // يخفي الماوس
    }

    void Update()
    {
        HandleMovement(); // حركة اللاعب
        HandleJump();     // القفز + الدبل جمب
        ApplyGravity();   // الجاذبية

        controller.Move(moveDirection * Time.deltaTime);

        HandleCameraFollow(); // الكاميرا تتبع اللاعب
    }

    // ------------------------
    // حركة اللاعب
    // ------------------------
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        if (inputDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg 
                                + cameraTransform.eulerAngles.y;

            float angle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, rotationSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            moveDirection.x = moveDir.x * speed;
            moveDirection.z = moveDir.z * speed;
        }
        else
        {
            moveDirection.x = 0;
            moveDirection.z = 0;
        }
    }

    // ------------------------
    // القفز ودبل جمب
    // ------------------------
    void HandleJump()
    {
        if (controller.isGrounded)
        {
            gravityVelocity = -2f; // يساعد اللاعب يثبت بالأرض
            canDoubleJump = true;  // إعادة تفعيل الدبل جمب

            if (Input.GetButtonDown("Jump"))
            {
                gravityVelocity = jumpForce; // قفزة أولى
            }
        }
        else
        {
            if (canDoubleJump && Input.GetButtonDown("Jump"))
            {
                gravityVelocity = doubleJumpForce; // دبل جمب
                canDoubleJump = false;
            }
        }

        moveDirection.y = gravityVelocity;
    }

    // ------------------------
    // الجاذبية
    // ------------------------
    void ApplyGravity()
    {
        gravityVelocity += Physics.gravity.y * Time.deltaTime;
    }

    // ------------------------
    // تتبع الكاميرا للاعب
    // ------------------------
    void HandleCameraFollow()
    {
        if (cameraTransform != null)
        {
            Vector3 camPos = transform.position + new Vector3(0, cameraHeight, -4);
            cameraTransform.position = camPos;
            cameraTransform.LookAt(transform);
        }
    }
    void Attack()
{
  
        Vector3 hitPos = transform.position + transform.forward * 1f;

        Collider[] hits = Physics.OverlapSphere(hitPos, 1f);

        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(1);
            }
        
    }
}
    public int health = 3;

   

public void TakeDamage(int damage)
{
    health -= damage;
    Debug.Log("Player got hit! Health = " + health);

    if (health <= 0)
    {
        Debug.Log("Player died!");
        // هنا ممكن نضيف موت لاحقاً
    }
}
}                   


