using UnityEngine;

public class EnemyChaseAttack : MonoBehaviour
{
    public Transform player;
    public Animator animator;

    public float moveSpeed = 3f;
    public float attackDistance = 1.5f;
    public float damage = 20f;
    public float attackCooldown = 1.2f;

    private float nextAttackTime;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            Chase();
        }
        else
        {
            Attack();
        }
    }

    void Chase()
    {
        animator.SetBool("IsMoving", true);

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        controller.Move(dir * moveSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 10f
        );
    }

    void Attack()
    {
        animator.SetBool("IsMoving", false);

        if (Time.time < nextAttackTime) return;

        animator.SetTrigger("Attack");
        nextAttackTime = Time.time + attackCooldown;

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null && !health.IsDead())
        {
            health.TakeDamage(damage);
        }
    }
}