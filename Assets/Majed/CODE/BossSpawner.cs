using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bossPrefab;

    [Header("Spawn Settings")]
    public float activationDistance = 10f;

    private bool hasSpawned = false;

    void Update()
    {
        if (hasSpawned || player == null || bossPrefab == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= activationDistance)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
        hasSpawned = true;
    }
}
