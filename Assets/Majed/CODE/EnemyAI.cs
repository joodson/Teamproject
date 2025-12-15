using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float attackDistance = 2f;
    public int damage = 10;

    private Animator anim;
    private bool isDead = false;
    private float attackCooldown = 1.5f;
    private float timer = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            anim.SetBool("isMoving", true);
            transform.LookAt(player);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            anim.SetBool("isMoving", false);

            timer += Time.deltaTime;
            if (timer >= attackCooldown)
            {
                anim.SetTrigger("Attack");

                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                    health.TakeDamage(damage);

                timer = 0;
            }
        }
    }

    public void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        Destroy(gameObject, 2f);
    }
}