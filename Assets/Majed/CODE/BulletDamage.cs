using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int damage = 35;      // مقدار الدمج للرأس أو الجسم
    public float lifeTime = 3f;  // وقت اختفاء الرصاصة

    void Start()
    {
        Destroy(gameObject, lifeTime); // تختفي لو ما أصابت شيء
    }

    void OnCollisionEnter(Collision col)
    {
        // إذا أصابت زومبي
        if (col.gameObject.CompareTag("Zombie"))
        {
            Health hp = col.gameObject.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }

        Destroy(gameObject); // الرصاصة تختفي بعد الاصطدام
    }
}
