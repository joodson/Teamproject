using UnityEngine;
using System.Collections;

public class SonicController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Attack")]
    public int attackDamage = 30;
    public float attackRange = 1.2f; // مدى الضرب (قدّام Sonic)
    public float attackDelay = 0.2f; // متى يحسب الضرب بعد بداية الأنيميشن

    Animator anim;
    CharacterController controller;

    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Attack();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        bool isMoving = dir.magnitude > 0;

        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            // دوران
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            // مشي
            controller.Move(dir * speed * Time.deltaTime);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0)) // زر الهجوم
        {
            anim.SetTrigger("Attack");
            StartCoroutine(DoAttack());
        }
    }

    IEnumerator DoAttack()
    {
        yield return new WaitForSeconds(attackDelay);

        // نطلق Ray أمام سونيك علشان نشوف إذا أصاب عدو
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up; // مستوى الصدر
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, attackRange))
        {
            // إذا لمس جسم فيه EnemyHealth
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.health -= attackDamage;

                if (enemy.health <= 0)
                    Destroy(enemy.gameObject);
            }
        }
    }
}

