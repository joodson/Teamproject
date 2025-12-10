using UnityEngine;
using UnityEngine.UI;

public class CrosshairUI : MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] private Image crosshairImage; // The crosshair sprite
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color aimColor = new Color(1f, 1f, 1f, 0.5f); // Semi-transparent when aiming

    [Header("Crosshair Spread (Recoil)")]
    [SerializeField] private float normalSpread = 20f; // Normal crosshair size
    [SerializeField] private float aimSpread = 10f;    // Tighter when aiming
    [SerializeField] private float maxSpread = 50f;    // Maximum spread when shooting
    [SerializeField] private float spreadRecoverySpeed = 5f;

    [Header("Dynamic Crosshair Parts (Optional)")]
    [SerializeField] private RectTransform topLine;
    [SerializeField] private RectTransform bottomLine;
    [SerializeField] private RectTransform leftLine;
    [SerializeField] private RectTransform rightLine;

    [Header("Hit Marker")]
    [SerializeField] private Image hitMarker; // Shows when you hit something
    [SerializeField] private float hitMarkerDuration = 0.2f;

    private float _currentSpread;
    private float _targetSpread;
    private bool _isAiming = false;
    private float _hitMarkerTimer = 0f;

    private void Start()
    {
        // Initialize spread
        _currentSpread = normalSpread;
        _targetSpread = normalSpread;

        // Hide hit marker at start
        if (hitMarker != null)
            hitMarker.enabled = false;
    }

    private void Update()
    {
        // Smoothly interpolate current spread to target spread
        _currentSpread = Mathf.Lerp(_currentSpread, _targetSpread, spreadRecoverySpeed * Time.deltaTime);

        // Update crosshair appearance
        UpdateCrosshairSpread();
        UpdateCrosshairColor();
        UpdateHitMarker();
    }

    private void UpdateCrosshairSpread()
    {
        // Update dynamic crosshair parts if they exist
        if (topLine != null)
            topLine.anchoredPosition = new Vector2(0f, _currentSpread);
        if (bottomLine != null)
            bottomLine.anchoredPosition = new Vector2(0f, -_currentSpread);
        if (leftLine != null)
            leftLine.anchoredPosition = new Vector2(-_currentSpread, 0f);
        if (rightLine != null)
            rightLine.anchoredPosition = new Vector2(_currentSpread, 0f);
    }

    private void UpdateCrosshairColor()
    {
        if (crosshairImage == null)
            return;

        // Change color based on aim state
        Color targetColor = _isAiming ? aimColor : normalColor;
        crosshairImage.color = Color.Lerp(crosshairImage.color, targetColor, 10f * Time.deltaTime);
    }

    private void UpdateHitMarker()
    {
        if (hitMarker == null)
            return;

        // Countdown hit marker timer
        if (_hitMarkerTimer > 0f)
        {
            _hitMarkerTimer -= Time.deltaTime;
            hitMarker.enabled = true;
        }
        else
        {
            hitMarker.enabled = false;
        }
    }

    // ========== PUBLIC METHODS ==========

    // Call this when player shoots
    public void OnShoot()
    {
        // Expand crosshair when shooting
        _targetSpread = Mathf.Min(_targetSpread + 5f, maxSpread);
    }

    // Call this when entering/exiting aim mode
    public void SetAiming(bool isAiming)
    {
        _isAiming = isAiming;
        _targetSpread = isAiming ? aimSpread : normalSpread;
    }

    // Call this when bullet hits something
    public void ShowHitMarker()
    {
        _hitMarkerTimer = hitMarkerDuration;
    }

    // Show/hide entire crosshair
    public void SetCrosshairVisible(bool visible)
    {
        if (crosshairImage != null)
            crosshairImage.enabled = visible;
    }
}