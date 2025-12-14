using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthEffectManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private string losingSceneName = "losing"; 

    [Header("Red Effect Settings")]
    [SerializeField] private Image redOverlay;
    [SerializeField] private float maxAlpha = 0.7f; // Maximum opacity when health is 0
    [SerializeField] private float pulseSpeed = 2f; // Speed of the pulsing effect

    private float currentHealth;
    private bool isDead = false;

    private bool shouldPulse = false;

    void Start()
    {
        // Create red overlay if not assigned
        if (redOverlay == null)
        {
            CreateRedOverlay();
        }


            currentHealth = maxHealth;

        UpdateRedEffect();
    }

    void Update()
    {

        // Check if player is dead
        if (currentHealth <= 0f && !isDead)
        {
            isDead = true;
            LoadLosingScene();
        }

        UpdateRedEffect();

        // Test keys (remove in production)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(5f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(5f);
        }
    }

    void UpdateRedEffect()
    {
        if (redOverlay == null) return;

        if (currentHealth < 100f)
        {
            // Calculate intensity based on health (0-20 range)
            float healthPercent = currentHealth / 100f;
            float baseAlpha = (1f - healthPercent) * maxAlpha;

            // Add pulsing effect for dramatic feel
            float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
            float finalAlpha = baseAlpha + (pulse * 0.2f * (1f - healthPercent));

            Color color = redOverlay.color;
            color.a = Mathf.Clamp01(finalAlpha);
            redOverlay.color = color;
        }
        else
        {
            // Fade out the effect
            Color color = redOverlay.color;
            color.a = Mathf.Lerp(color.a, 0f, Time.deltaTime * 3f);
            redOverlay.color = color;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(0f, currentHealth - damage);

        Debug.Log($"Health: {currentHealth}");
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);

        Debug.Log($"Health: {currentHealth}");
    }

    void CreateRedOverlay()
    {
        // Create a Canvas if none exists
        Canvas canvas = FindObjectOfType<Canvas>();

        // Create the red overlay image
        GameObject overlayObj = new GameObject("RedOverlay");
        overlayObj.transform.SetParent(canvas.transform, false);

        redOverlay = overlayObj.AddComponent<Image>();

        // Create a radial gradient material for vignette effect
        // Using a simple white sprite with color tint
        redOverlay.color = new Color(1f, 0f, 0f, 0f); // Red with 0 alpha
        redOverlay.raycastTarget = false; // Don't block UI interactions

        // Set the image type to create vignette effect
        // We'll use a custom sprite with radial gradient
        Texture2D vignetteTexture = CreateVignetteTexture();
        Sprite vignetteSprite = Sprite.Create(vignetteTexture, new Rect(0, 0, vignetteTexture.width, vignetteTexture.height), new Vector2(0.5f, 0.5f));
        redOverlay.sprite = vignetteSprite;

        // Make it cover the entire screen
        RectTransform rt = overlayObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    Texture2D CreateVignetteTexture()
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

    void LoadLosingScene()
    {
        Debug.Log("Player died! Loading losing scene...");
        SceneManager.LoadScene(losingSceneName);
    }
}