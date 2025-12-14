using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 2f;        // How close to attack
    [SerializeField] private float attackCooldown = 1f;     // Time between attacks

    [Header("References")]
    [SerializeField] private Transform player;              // Reference to player

    // Private variables
    private float _nextAttackTime = 0f;

    private void Start()
    {
        // getting the player object by looking for the tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        // having the player transform in the varuable player
        player = playerObj.transform;
    }

    private void Update()
    {
        // Check if player is in attack range
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange && Time.time >= _nextAttackTime)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        _nextAttackTime = Time.time + attackCooldown;

        // Deal damage to player
        DealDamage();
    }

    // This method actually deals the damage
    public void DealDamage()
    {
        if (PlayerHealth.Instance != null)
        {
            // Check if player is still in range
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                PlayerHealth.Instance.TakeDamage(attackDamage);
            }
        }
    }
}