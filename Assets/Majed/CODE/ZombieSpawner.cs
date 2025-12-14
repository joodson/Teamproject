using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;          // اللاعب
    public GameObject zombiePrefab;   // Prefab الزومبي

    [Header("Spawn Settings")]
    public int maxZombies = 20;
    public float spawnRate = 2f;
    public float spawnRadius = 5f;
    public float activationDistance = 15f; // اللاعب لازم يكون قريب

    private float nextSpawnTime;

    void Update()
    {
        if (player == null || zombiePrefab == null)
            return;

        // إذا اللاعب بعيد → لا تسباون
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance > activationDistance)
            return;

        if (Time.time >= nextSpawnTime)
        {
            TrySpawnZombie();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void TrySpawnZombie()
    {
        int currentZombies = GameObject.FindGameObjectsWithTag("Zombie").Length;
        if (currentZombies >= maxZombies)
            return;

        // مكان عشوائي حول نقطة السباون
        Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPos.y = transform.position.y;

        GameObject zombie = Instantiate(zombiePrefab, spawnPos, Quaternion.identity);

        // ربط اللاعب للزومبي
        ZombieFollow follow = zombie.GetComponent<ZombieFollow>();
        if (follow != null)
            follow.player = player;
    }

    // عشان تشوف الرينج في Scene
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
