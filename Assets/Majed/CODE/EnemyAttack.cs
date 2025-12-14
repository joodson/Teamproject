using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage = 10f;
    public float attackRange = 1.5f;
    public float attackRate = 1f;

    private float nextAttackTime;
    private Transform player;

    void Start()
    {
        // نجيب اللاعب عن طريق الـ Tag
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }

    void Attack()
    {
        // ننادي PlayerHealth
        PlayerHealth health = player.GetComponent<PlayerHealth>();

        if (health != null && !health.IsDead())
        {
            health.TakeDamage(damage);
        }
    }
}
