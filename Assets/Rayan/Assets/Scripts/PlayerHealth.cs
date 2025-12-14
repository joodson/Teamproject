using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Health Regeneration")]
    [SerializeField] private bool enableRegeneration = true;
    [SerializeField] private float regenRate = 5f; // Health per second
    [SerializeField] private float regenDelay = 3f; // Seconds before regen starts after taking damage

    [Header("Damage Overlay")]
    [SerializeField] private DamageOverlay damageOverlay; // Reference to the blood overlay script

    // Private variables
    private float _timeSinceLastDamage = 0f;
    private bool _isDead = false;

    // Singleton
    public static PlayerHealth Instance;

    private void Awake()
    {
        // Singleton setup - only one PlayerHealth can exist
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // Start with full health
        currentHealth = maxHealth;

        // Update overlay at start (should be invisible)
        UpdateDamageOverlay();
    }

    private void Update()
    {
        if (_isDead)
            return;

        // Handle health regeneration
        HandleRegeneration();

        // Update the blood overlay based on current health
        UpdateDamageOverlay();
    }

    private void HandleRegeneration()
    {
        // If regeneration is disabled, skip
        if (!enableRegeneration)
            return;

        // If health is already full, don't regenerate
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth; // Clamp to max
            return;
        }

        // Count time since last damage
        _timeSinceLastDamage += Time.deltaTime;

        // Only regenerate after the delay has passed
        if (_timeSinceLastDamage >= regenDelay)
        {
            // Add health over time
            currentHealth += regenRate * Time.deltaTime;

            // Don't exceed max health
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }
    }

    // ========== PUBLIC METHODS ==========
    // call when taking damage from zombie
    public void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        // Reduce health
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f); // Don't go below 0

        // Reset regeneration timer (stops healing for 3 seconds)
        _timeSinceLastDamage = 0f;

        // Show damage effect
        if (damageOverlay != null)
            damageOverlay.ShowDamageFlash();

        // Check if dead
        if (currentHealth <= 0f)
        {
            Die();
        }
    }


    // ========== DEATH AND RESPAWN ==========

    private void Die()
    {
        _isDead = true;

        // Disable player movement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        // Disable weapon
        WeaponController weapon = GetComponentInChildren<WeaponController>();
        if (weapon != null)
            weapon.enabled = false;

        // Show full red overlay (player is dead)
        if (damageOverlay != null)
            damageOverlay.ShowDeathOverlay();
    }


    // ========== DAMAGE OVERLAY ==========

    private void UpdateDamageOverlay()
    {
        if (damageOverlay != null)
        {
            // Calculate how much health is missing (0 = full health, 1 = no health)
            float damagePercent = 1f - (currentHealth / maxHealth);

            // Update the overlay intensity
            damageOverlay.SetOverlayIntensity(damagePercent);
        }
    }

    // ========== GETTERS ==========

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public bool IsDead() => _isDead;
}