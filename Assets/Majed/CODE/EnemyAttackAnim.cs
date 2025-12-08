using UnityEngine;
using System.Collections;

public class EnemyAttackAnim : MonoBehaviour
{
    public Transform player;     
    public float attackRange = 2f;  
    public float attackDelay = 1.5f;   // الوقت بين كل ضربة

    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(AttackLoop()); // ✅ تشغيل اللوب
    }

    IEnumerator AttackLoop()
    {
        while (true) // 🔁 لوب لا نهائي
        {
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.position); // ✅ تعريف distance بشكل صحيح

                if (distance <= attackRange)
                {
                    int randomAttack = Random.Range(0, 4); // ✅ من 1 إلى 3 فقط
                    anim.SetInteger("AttackType", randomAttack);
                }
                else
                {
                    anim.SetInteger("AttackType", 0); // ✅ يرجع للمشي
                }
            }

            yield return new WaitForSeconds(attackDelay); // ✅ وقت الانتظار بين كل ضربة
        }
    }
}