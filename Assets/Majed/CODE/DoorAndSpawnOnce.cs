using UnityEngine;

public class DoorPlayerSpawn : MonoBehaviour
{
    [Header("Player")]
    public GameObject hiddenPlayer;   // اللاعب المخفي (Disabled)

    [Header("Door")]
    public GameObject door;           // الباب
    public float interactDistance = 3f;

    [Header("Spawn")]
    public GameObject monsterPrefab;  // الوحش
    public Transform spawnPoint;      // مكان السبون

    private bool spawned = false;
    private Transform player;         // اللاعب المتحكم (الكاميرا)

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            DoAll();
        }
    }

    void DoAll()
    {
        // إظهار اللاعب المخفي
        if (hiddenPlayer != null)
            hiddenPlayer.SetActive(true);

        // إخفاء الباب
        if (door != null)
            door.SetActive(false);

        // سباون الوحش مرة وحدة فقط
        if (!spawned && monsterPrefab != null && spawnPoint != null)
        {
            Instantiate(monsterPrefab, spawnPoint.position, spawnPoint.rotation);
            spawned = true;
        }
    }
}