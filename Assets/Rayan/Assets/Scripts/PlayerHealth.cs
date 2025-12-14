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

    [Header("Death Settings")]
    [SerializeField] private bool respawnOnDeath = false;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private Transform respawnPoint;

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

    /// <summary>
    /// Call this when the player takes damage (e.g., from a zombie attack)
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        // Reduce health
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f); // Don't go below 0

        // Reset regeneration timer (stops healing for 3 seconds)
        _timeSinceLastDamage = 0f;

        // Show damage effect (flash the overlay)
        if (damageOverlay != null)
            damageOverlay.ShowDamageFlash();

        // Check if dead
        if (currentHealth <= 0f)
        {
            Die();
        }

        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
    }

    /// <summary>
    /// Heal the player by a specific amount
    /// </summary>
    public void Heal(float amount)
    {
        if (_isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount}. Current health: {currentHealth}");
    }

    /// <summary>
    /// Fully restore player health
    /// </summary>
    public void FullHeal()
    {
        currentHealth = maxHealth;
        Debug.Log("Player fully healed!");
    }

    // ========== DEATH AND RESPAWN ==========

    private void Die()
    {
        _isDead = true;
        Debug.Log("Player died!");

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

        // Respawn if enabled
        if (respawnOnDeath)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
    }

    private void Respawn()
    {
        // Restore health
        currentHealth = maxHealth;
        _isDead = false;
        _timeSinceLastDamage = 0f;

        // Re-enable player controls
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = true;

        WeaponController weapon = GetComponentInChildren<WeaponController>();
        if (weapon != null)
            weapon.enabled = true;

        // Move to respawn point
        if (respawnPoint != null)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false; // Disable before teleporting
                transform.position = respawnPoint.position;
                transform.rotation = respawnPoint.rotation;
                controller.enabled = true; // Re-enable after teleporting
            }
            else
            {
                transform.position = respawnPoint.position;
                transform.rotation = respawnPoint.rotation;
            }
        }

        Debug.Log("Player respawned!");
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

    // ========== SETTERS ==========

    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = newMaxHealth;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void SetRegenRate(float newRate)
    {
        regenRate = newRate;
    }

    public void SetRegenDelay(float newDelay)
    {
        regenDelay = newDelay;
    }

    public void EnableRegeneration(bool enable)
    {
        enableRegeneration = enable;
    }
}