using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab;   // نسخة الزومبي
    public Transform player;          // اللاعب
    public int maxZombies = 20;       // الحد الأعلى للزومبي
    public float spawnRate = 2f;      // كل كم ثانية يرسبن واحد جديد
    public float spawnRadius = 10f;   // المسافة بينك وبين مكان ظهور الزومبي

    private float nextSpawnTime = 0f;

    void Update()
    {
        // لا يرسبن إلا بعد الوقت المحدد
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnZombie();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void TrySpawnZombie()
    {
        // عد عدد الزومبي الموجودين
        int currentZombies = GameObject.FindGameObjectsWithTag("Zombie").Length;

        // لو العدد وصل 20 لا يرسبن
        if (currentZombies >= maxZombies)
            return;

        // حدد مكان عشوائي حول اللاعب
        Vector3 spawnPos = player.position + Random.insideUnitSphere * spawnRadius;
        spawnPos.y = player.position.y;

        // أنشئ الزومبي
        GameObject newZombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        // اربط اللاعب داخل سكربت الزومبي
        ZombieFollow follow = newZombie.GetComponent<ZombieFollow>();
        follow.player = player;
    }
  
}