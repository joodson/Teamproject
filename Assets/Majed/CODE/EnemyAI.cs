using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;           
    public float moveSpeed = 2.5f;     
    public float chaseRange = 10f;     
    public float attackRange = 1.5f;   
    public float attackCooldown = 1.2f;

    private float nextAttackTime = 0f; 
    private Animator anim;
   


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 1) اللاعب بعيد = العدو يقف
        if (distance > chaseRange)
        {
            anim.SetBool("isMoving", false);
            return;
        }

        // 2) اللاعب داخل نطاق التحرك = العدو يمشي باتجاه اللاعب
        if (distance > attackRange)
        {
            anim.SetBool("isMoving", true);
            transform.LookAt(player);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            return;
        }

        // 3) اللاعب قريب جدًا = العدو يهاجم
        anim.SetBool("isMoving", false);

        if (Time.time >= nextAttackTime)
        {
            anim.SetTrigger("Attack");
            player.GetComponent<playerMovemnt>().TakeDamage(1);

            nextAttackTime = Time.time + attackCooldown;
        }
    }
    public void Die()
{
    // شغل أنيميشن الموت
    anim.SetTrigger("Death");

    // أوقف الحركة والهجوم
    this.enabled = false;

    // حذف العدو بعد ثانية (اختياري)
    Destroy(gameObject, 2f);
}
}