using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    [Header("Surface Type")]
    [SerializeField] private SurfaceType surfaceType = SurfaceType.Concrete;

    [Header("Impact Effects")]
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private float effectLifetime = 2f;

    [Header("Bullet Hole Settings")]
    [SerializeField] private GameObject bulletHoleDecal;
    [SerializeField] private bool spawnBulletHole = true;

    public void OnHit(Vector3 hitPoint, Vector3 hitNormal)
    {
        PlayImpactSound(hitPoint);
        SpawnImpactEffect(hitPoint, hitNormal);

        if (spawnBulletHole && bulletHoleDecal != null)
        {
            SpawnBulletHole(hitPoint, hitNormal);
        }
    }

    private void PlayImpactSound(Vector3 position)
    {
        if (WeaponAudioManager.Instance != null)
        {
            WeaponAudioManager.Instance.PlayImpactSound(surfaceType, position);
        }
    }

    private void SpawnImpactEffect(Vector3 position, Vector3 normal)
    {
        if (impactEffect == null)
            return;

        GameObject effect = Instantiate(impactEffect, position, Quaternion.LookRotation(normal));
        Destroy(effect, effectLifetime);
    }

    private void SpawnBulletHole(Vector3 position, Vector3 normal)
    {
        Quaternion rotation = Quaternion.LookRotation(normal);
        Vector3 spawnPosition = position + normal * 0.05f;

        GameObject hole = Instantiate(bulletHoleDecal, spawnPosition, rotation);

        // Fix the tiny scale from the prefab
        Transform meshChild = hole.transform.GetChild(0);
        if (meshChild != null)
        {
            meshChild.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        // Ensure pooling is disabled
        BulletHole bhScript = hole.GetComponent<BulletHole>();
        if (bhScript != null)
        {
            bhScript.usePooling = false;
        }
    }

    public void SetSurfaceType(SurfaceType type)
    {
        surfaceType = type;
    }

    public SurfaceType GetSurfaceType()
    {
        return surfaceType;
    }
}

public enum SurfaceType
{
    Concrete,
    Metal,
    Wood,
    Ground,
    Flesh,
    Glass,
    Water
}