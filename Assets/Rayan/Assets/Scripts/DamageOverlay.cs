using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the red blood/damage overlay on the screen (like Call of Duty Zombies).
/// Shows more red when health is low, fades as health regenerates.
/// </summary>
public class DamageOverlay : MonoBehaviour
{
    [Header("Overlay Image")]
    [SerializeField] private Image overlayImage; // The UI Image with red vignette texture

    [Header("Overlay Settings")]
    [SerializeField] private Color overlayColor = new Color(1f, 0f, 0f, 0.8f); // Red color with transparency
    [SerializeField] private float maxAlpha = 0.8f; // Maximum opacity when almost dead (0-1)
    [SerializeField] private float minAlpha = 0f; // Minimum opacity at full health (0-1)

    [Header("Fade Settings")]
    [SerializeField] private float fadeSpeed = 2f; // How fast the overlay fades in/out

    [Header("Damage Flash")]
    [SerializeField] private bool enableDamageFlash = true; // Flash red when hit
    [SerializeField] private float flashIntensity = 0.5f; // How bright the flash is
    [SerializeField] private float flashDuration = 0.15f; // How long the flash lasts

    // Private variables
    private float _targetAlpha = 0f; // The alpha we're fading to
    private float _currentAlpha = 0f; // Current alpha value
    private float _flashTimer = 0f; // Timer for damage flash

    private void Start()
    {
        // Make sure we have an overlay image
        if (overlayImage == null)
        {
            Debug.LogError("DamageOverlay: No overlay image assigned! Please assign a UI Image.");
            return;
        }

        // Start with overlay invisible
        _currentAlpha = 0f;
        _targetAlpha = 0f;
        UpdateOverlayVisual();
    }

    private void Update()
    {
        // Smoothly fade towards target alpha
        _currentAlpha = Mathf.Lerp(_currentAlpha, _targetAlpha, fadeSpeed * Time.deltaTime);

        // Handle damage flash
        if (_flashTimer > 0f)
        {
            _flashTimer -= Time.deltaTime;
        }

        // Update the visual
        UpdateOverlayVisual();
    }

    /// <summary>
    /// Set the intensity of the overlay based on missing health (0 = full health, 1 = no health)
    /// </summary>
    public void SetOverlayIntensity(float damagePercent)
    {
        // Clamp between 0 and 1
        damagePercent = Mathf.Clamp01(damagePercent);

        // Calculate target alpha (more damage = more visible)
        _targetAlpha = Mathf.Lerp(minAlpha, maxAlpha, damagePercent);
    }

    /// <summary>
    /// Flash the overlay when taking damage
    /// </summary>
    public void ShowDamageFlash()
    {
        if (enableDamageFlash)
        {
            _flashTimer = flashDuration;
        }
    }

    /// <summary>
    /// Show full red overlay when dead
    /// </summary>
    public void ShowDeathOverlay()
    {
        _targetAlpha = maxAlpha;
        _currentAlpha = maxAlpha;
        UpdateOverlayVisual();
    }

    /// <summary>
    /// Update the overlay image's color and transparency
    /// </summary>
    private void UpdateOverlayVisual()
    {
        if (overlayImage == null)
            return;

        // Calculate final alpha (add flash effect if active)
        float finalAlpha = _currentAlpha;

        if (_flashTimer > 0f)
        {
            // Add extra brightness during flash
            float flashProgress = _flashTimer / flashDuration;
            finalAlpha += flashIntensity * flashProgress;
            finalAlpha = Mathf.Clamp01(finalAlpha); // Keep between 0 and 1
        }

        // Apply color with calculated alpha
        Color newColor = overlayColor;
        newColor.a = finalAlpha;
        overlayImage.color = newColor;
    }
}