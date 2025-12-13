using UnityEngine;

public class WeaponController : MonoBehaviour
{
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
    [SerializeField] private Transform firePoint;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Audio")]
    [SerializeField] private WeaponAudioManager audioManager;

    [Header("UI")]
    [SerializeField] private CrosshairUI crosshairUI;

    [Header("Player Animation")]
    [SerializeField] private Animator anim;

    private float _nextFireTime = 0f;
    private bool _isReloading = false;
    private float _currentRecoil = 0f;

    private void Update()
    {
        if (_isReloading)
            return;

        HandleShooting();
        HandleReload();
        HandleRecoil();
        HandleAimCrosshair();
    }

    private void HandleAimCrosshair()
    {
        ThirdPersonCamera cam = playerCamera.GetComponent<ThirdPersonCamera>();
            
        crosshairUI.SetAiming(cam.IsAiming());
    }

    private void HandleShooting()
    {
        if (Input.GetButton("Fire1") && Time.time >= _nextFireTime)
        {
            Shoot();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            anim.SetBool("Shooting", false);
        }
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
        {
            PlayEmptySound();
            return;
        }

        currentAmmo--;
        _nextFireTime = Time.time + fireRate;
        _currentRecoil += recoilAmount;

        PlayMuzzleFlash();
        PlayShootSound();

        anim.SetBool("Shooting", true);

        if (crosshairUI != null)
            crosshairUI.OnShoot();

        PerformRaycast();
    }

    private void PerformRaycast()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            HandleBulletImpact(hit);
        }
    }

    private void HandleBulletImpact(RaycastHit hit)
    {
        if (crosshairUI != null)
            crosshairUI.ShowHitMarker();

        BulletImpact impactScript = hit.collider.GetComponent<BulletImpact>();

        if (impactScript != null)
        {
            impactScript.OnHit(hit.point, hit.normal);
        }
    }


    private void HandleReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo && reserveAmmo > 0)
        {
            StartReload();
        }

        if (currentAmmo <= 0 && reserveAmmo > 0 && Input.GetButton("Fire1"))
        {
            StartReload();
        }
    }

    private void StartReload()
    {
        if (_isReloading)
            return;

        _isReloading = true;
        PlayReloadSound();

        Invoke(nameof(FinishReload), reloadTime);
    }

    private void FinishReload()
    {
        int ammoNeeded = maxAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        _isReloading = false;
    }

    private void HandleRecoil()
    {
        if (_currentRecoil > 0)
        {
            _currentRecoil = Mathf.Lerp(_currentRecoil, 0f, recoilSpeed * Time.deltaTime);

            if (playerCamera != null)
            {
                playerCamera.transform.localEulerAngles += new Vector3(-_currentRecoil * 0.1f, 0f, 0f);
            }
        }
    }

    private void PlayMuzzleFlash()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
    }

    private void PlayShootSound()
    {
        if (audioManager != null)
            audioManager.PlayShootSound();
    }

    private void PlayReloadSound()
    {
        if (audioManager != null)
            audioManager.PlayReloadSound();
    }

    private void PlayEmptySound()
    {
        if (audioManager != null)
            audioManager.PlayEmptySound();
    }

    public int GetCurrentAmmo() => currentAmmo;
    public int GetMaxAmmo() => maxAmmo;
    public int GetReserveAmmo() => reserveAmmo;
    public bool IsReloading() => _isReloading;

    public void AddAmmo(int amount)
    {
        reserveAmmo += amount;
    }
}