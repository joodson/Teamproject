using UnityEngine;

public class WeaponController : MonoBehaviour
{
    // 🔒 التحكم بالسلاح (المينيو / اللعب)
    public static bool CanShoot = false;

    [Header("Weapon Stats")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float range = 100f;

    [Header("Ammo")]
    [SerializeField] private int maxAmmo = 30;
    [SerializeField] private int currentAmmo = 30;
    [SerializeField] private int reserveAmmo = 90;
    [SerializeField] private float reloadTime = 2f;

    [Header("Recoil")]
    [SerializeField] private float recoilAmount = 2f;
    [SerializeField] private float recoilSpeed = 10f;

    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Audio")]
    [SerializeField] private WeaponAudioManager audioManager;

    private float nextFireTime;
    private bool isReloading;
    private float currentRecoil;

    void Update()
    {
        // ❌ ممنوع السلاح في المينيو
        if (!CanShoot)
            return;

        if (isReloading)
            return;

        HandleShoot();
        HandleReload();
        HandleRecoil();
    }

    // ===================== SHOOT =====================
    void HandleShoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            audioManager?.PlayEmptySound();
            return;
        }

        currentAmmo--;
        nextFireTime = Time.time + fireRate;
        currentRecoil += recoilAmount;

        muzzleFlash?.Play();
        audioManager?.PlayShootSound();

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    // ===================== RELOAD =====================
    void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartReload();
        }
    }

    void StartReload()
    {
        isReloading = true;
        audioManager?.PlayReloadSound();
        Invoke(nameof(FinishReload), reloadTime);
    }

    void FinishReload()
    {
        int need = maxAmmo - currentAmmo;
        int load = Mathf.Min(need, reserveAmmo);

        currentAmmo += load;
        reserveAmmo -= load;

        isReloading = false;
    }

    // ===================== RECOIL =====================
    void HandleRecoil()
    {
        if (currentRecoil > 0)
        {
            currentRecoil = Mathf.Lerp(currentRecoil, 0, recoilSpeed * Time.deltaTime);
            playerCamera.transform.localEulerAngles += Vector3.left * currentRecoil * 0.1f;
        }
    }
}