using UnityEngine;

public class MoveAndDestroy : MonoBehaviour
{
    public Transform targetPoint;          // النقطة الثانية
    public float speed = 5f;               // سرعة الحركة
    public float destroyDistance = 0.1f;   // متى ينحذف
    public float lifeTime = 4f;             // 🔥 يختفي بعد 4 ثواني

    void Start()
    {
        // حذف تلقائي بعد 4 ثواني
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (targetPoint == null) return;

        // تحريك الأوبجكت
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPoint.position,
            speed * Time.deltaTime
        );

        // إذا وصل للنقطة → ينحذف
        if (Vector3.Distance(transform.position, targetPoint.position) <= destroyDistance)
        {
            Destroy(gameObject);
        }
    }

    // 🔥 إذا صقع بلاعب
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}