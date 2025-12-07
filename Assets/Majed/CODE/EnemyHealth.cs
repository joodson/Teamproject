using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;
    private EnemyAI ai;

    void Start()
    {
        ai = GetComponent<EnemyAI>();
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            ai.Die();
        }
    }
    public void Die()
{
    // شغل أنيميشن الموت
    ai.Die();


    // أوقف الحركة والهجوم
    this.enabled = false;

    // حذف العدو بعد ثانية (اختياري)
    Destroy(gameObject, 2f);
}
}