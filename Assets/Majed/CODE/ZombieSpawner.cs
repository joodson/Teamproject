using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public Transform player;             // اللاعب
    public int maxZombies = 20;          // الحد الأعلى للزومبي
    public float spawnRate = 2f;         // كل كم ثانية يرسبن واحد جديد
    public float spawnRadius = 10f;      // مسافة ظهور الزومبي حول اللاعب

    private float nextSpawnTime = 0f;
    private GameObject zombieTemplate;   // النسخة الأساسية اللي بنستنسخ منها

    void Start()
    {
        // نحفظ نسخة من نفس الكائن كـ Template
        zombieTemplate = gameObject;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnZombie();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void TrySpawnZombie()
    {
        // نحسب عدد الزومبي المتواجدين
        int currentZombies = GameObject.FindGameObjectsWithTag("Zombie").Length;

        if (currentZombies >= maxZombies)
            return;

        // تحديد موقع عشوائي حول اللاعب
        Vector3 spawnPos = player.position + Random.insideUnitSphere * spawnRadius;
        spawnPos.y = player.position.y;

        // استنساخ الزومبي من نفس الكائن اللي المركب عليه السكربت
        GameObject newZombie = Instantiate(zombieTemplate, spawnPos, zombieTemplate.transform.rotation);

        // نحرص ألا نعبث بالـ spawner نفسه
        newZombie.GetComponent<ZombieSpawner>().enabled = false;

        // ربط اللاعب داخل سكربت متابعة الزومبي
        ZombieFollow follow = newZombie.GetComponent<ZombieFollow>();
        follow.player = player;
    }
}