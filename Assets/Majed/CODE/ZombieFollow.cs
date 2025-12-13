using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ZombieFollow : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public float stopDistance = 1.5f;

    public float gravity = -20f;   // 🔥 الجاذبية
    private float yVelocity;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (player == null) return;

        // ✅ جاذبية
        if (controller.isGrounded)
        {
            yVelocity = -2f; // يثبّت على الأرض
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        Vector3 move = Vector3.zero;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            Vector3 dir = player.position - transform.position;
            dir.y = 0;
            dir.Normalize();

            // دوران
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                8f * Time.deltaTime
            );

            move = dir * moveSpeed;
        }

        // دمج الحركة + الجاذبية
        move.y = yVelocity;

        controller.Move(move * Time.deltaTime);
    }
}