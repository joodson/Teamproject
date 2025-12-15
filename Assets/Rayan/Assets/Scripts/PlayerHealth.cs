using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Health Regeneration")]
    [SerializeField] private bool enableRegeneration = true;
    [SerializeField] private float regenRate = 5f; // Health per second
    [SerializeField] private float regenDelay = 3f; // Seconds before regen starts after taking damage

    [Header("Red Overlay Settings")]
    [SerializeField] private Image redOverlay;
    [SerializeField] private float maxAlpha = 0.7f; // Maximum opacity when health is 0
    [SerializeField] private float pulseSpeed = 2f; // Speed of the pulsing effect
    [SerializeField] private bool createOverlayAutomatically = true;

    [Header("Death Settings")]
    [SerializeField] private string losingSceneName = "losing";

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

        // Create red overlay if not assigned and auto-creation is enabled
        if (redOverlay == null && createOverlayAutomatically)
        {
            CreateRedOverlay();
        }

        // Update overlay at start (should be invisible)
        UpdateRedOverlay();
    }

    private void Update()
    {
        if (_isDead)
            return;

        // Handle health regeneration
        HandleRegeneration();

        // Update the red overlay based on current health
        UpdateRedOverlay();
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

    public void TakeDamage(float damage)
    {
        if (_isDead)
            return;

        // Reduce health
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0f); // Don't go below 0

        // Reset regeneration timer (stops healing for regenDelay seconds)
        _timeSinceLastDamage = 0f;

        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");

        // Check if dead
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (_isDead)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        Debug.Log($"Player healed {amount}. Health: {currentHealth}/{maxHealth}");
    }

    // ========== RED OVERLAY SYSTEM ==========

    private void UpdateRedOverlay()
    {
        if (redOverlay == null)
            return;

        if (currentHealth < maxHealth)
        {
            // Calculate intensity based on health
            float healthPercent = currentHealth / maxHealth;
            float baseAlpha = (1f - healthPercent) * maxAlpha;

            // Add pulsing effect for dramatic feel when health is low
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float finalAlpha = baseAlpha + (pulse * 0.2f * (1f - healthPercent));

            Color color = redOverlay.color;
            color.a = Mathf.Clamp01(finalAlpha);
            redOverlay.color = color;
        }
        else
        {
            // Fade out the effect when at full health
            Color color = redOverlay.color;
            color.a = Mathf.Lerp(color.a, 0f, Time.deltaTime * 3f);
            redOverlay.color = color;
        }
    }

    private void CreateRedOverlay()
    {
        // Find or create a Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // Create the red overlay image
        GameObject overlayObj = new GameObject("RedOverlay");
        overlayObj.transform.SetParent(canvas.transform, false);

        redOverlay = overlayObj.AddComponent<Image>();

        // Create vignette texture and sprite
        Texture2D vignetteTexture = CreateVignetteTexture();
        Sprite vignetteSprite = Sprite.Create(
            vignetteTexture,
            new Rect(0, 0, vignetteTexture.width, vignetteTexture.height),
            new Vector2(0.5f, 0.5f)
        );
        redOverlay.sprite = vignetteSprite;

        redOverlay.color = new Color(1f, 0f, 0f, 0f); // Red with 0 alpha initially
        redOverlay.raycastTarget = false; // Don't block UI interactions

        // Make it cover the entire screen
        RectTransform rt = overlayObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        Debug.Log("Red overlay created automatically");
    }

    private Texture2D CreateVignetteTexture()
    {
        int size = 512;
        Texture2D texture = new Texture2D(size, size);

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float maxDistance = Vector2.Distance(Vector2.zero, center);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 pos = new Vector2(x, y);
                float distance = Vector2.Distance(pos, center);

                // Create vignette effect - darker at edges, transparent in center
                float normalizedDistance = distance / maxDistance;
                float alpha = Mathf.Pow(normalizedDistance, 1.5f); // Power for smoother falloff

                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        texture.Apply();
        return texture;
    }

    // ========== DEATH ==========

    private void Die()
    {
        _isDead = true;

        Debug.Log("Player died! Loading losing scene...");

        // Disable player movement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        // Disable weapon
        WeaponController weapon = GetComponentInChildren<WeaponController>();
        if (weapon != null)
            weapon.enabled = false;

        // Show full red overlay (player is dead)
        if (redOverlay != null)
        {
            Color color = redOverlay.color;
            color.a = 1f; // Full opacity
            redOverlay.color = color;
        }

        // Load losing scene after a short delay
        Invoke(nameof(LoadLosingScene), 2f);
    }

    private void LoadLosingScene()
    {
        SceneManager.LoadScene(losingSceneName);
    }

    // ========== GETTERS ==========

    public float GetCurrentHealth() => currentHealth;
    public float GetMaxHealth() => maxHealth;
    public float GetHealthPercentage() => currentHealth / maxHealth;
    public bool IsDead() => _isDead;
}