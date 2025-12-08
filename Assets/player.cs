using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpHeight = 2;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;

    // the double jump 
    public int maxJumps = 2;
    private int jumpCount;

    // the audio
    public AudioSource walkSource;
    public AudioSource backgroundMusic;
    public AudioSource jumpMusic;

    public float walkStepInterval = 0.5f;
    private float walkTimer;

   
    void Start()
    {

       

        // 
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("PlayerController requires a CharacterController component!");
            enabled = false;
        }

        // music will repet 
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

       
    }

    void Update()
    {
       


        // like grouned cheker
        isGrounded = controller.isGrounded;

        // Reset jumps when on the ground
        if (isGrounded)
        {
            if (playerVelocity.y < 0)
                playerVelocity.y = -2f;

            jumpCount = 0; // Reset jumps on ground
        }

        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        HandleWalkingSound(horizontalInput, verticalInput);

        // double jump
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpCount++;

            if (jumpMusic != null)
                jumpMusic.Play();
        }

        // gravity
        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

       
    }


    void HandleWalkingSound(float horizontal, float vertical)
    {
        bool isMoving = (horizontal != 0 || vertical != 0) && isGrounded;

        if (isMoving)
        {
            walkTimer -= Time.deltaTime;
            if (walkTimer <= 0f)
            {
                walkSource.Play();
                walkTimer = walkStepInterval;
            }
        }
        else
        {
            walkTimer = 0.1f;
        }
    }

   
    

    // called from UIrestart button
    public void RestartLevel()
    {
        Time.timeScale = 1f; // unpause
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // reload scene
    }

    // makes the player take dameg 
   
    // when the player dies a game over scren apers(UI) 
    

}
