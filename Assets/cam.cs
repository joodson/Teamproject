using UnityEngine;

public class cam : MonoBehaviour
{
    public float mouseSensitivity = 300f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        // Hide and lock mouse in center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // makes the camera go up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f); // Prevents  upsidedown camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // player left and right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
