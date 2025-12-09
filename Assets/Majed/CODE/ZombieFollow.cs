using UnityEngine;

public class ZombieFollow : MonoBehaviour
{
    public Transform player;       // هدف المطاردة (اللاعب)
    public float moveSpeed = 2f;   // سرعة الزومبي
    public float stopDistance = 1.5f; // مسافة التوقف
    public float pushForce = 0.1f; // قوة دفع الزومبي لبعض

    void Update()
    {
        if (player == null) return;

        // حساب المسافة بين الزومبي واللاعب
        float distance = Vector3.Distance(transform.position, player.position);

        // إذا اللاعب بعيد → امشِ نحوه
        if (distance > stopDistance)
        {
            // الزومبي يلتفت نحو اللاعب
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookPos), 0.1f);

            // يمشي باتجاه اللاعب
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else
        {
            // ما يتحرك، لكن ينظر للاعب
            Vector3 lookPos = player.position - transform.position;
            lookPos.y = 0;
            transform.rotation = Quaternion.LookRotation(lookPos);
        }
    }

    // 🔥 منع الزومبي من الدخول داخل بعضهم
    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.CompareTag("Zombie"))
        {
            // اتجاه الدفع
            Vector3 pushDir = transform.position - col.transform.position;
            pushDir.y = 0;

            // دفع بسيط للخارج
            transform.position += pushDir.normalized * pushForce;
        }
    }
}