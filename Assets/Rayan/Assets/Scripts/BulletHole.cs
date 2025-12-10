using UnityEngine;

public class BulletHole : MonoBehaviour
{
    public GameObject bulletHoleMesh;
    public bool usePooling = false;
    public float lifetime = 28.0f;
    public float startFadeTime = 10.0f;
    public float fadeRate = 0.001f;

    private float timer; 
    private Color targetColor;

    void Start()
    {
        if (bulletHoleMesh == null)
            return;

        timer = 0.0f;

        Renderer renderer = bulletHoleMesh.GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            targetColor = renderer.material.color;
            targetColor.a = 0.0f;
        }

        AttachToParent();
    }

    void Update()
    {
        if (!usePooling)
            FadeAndDieOverTime();
    }

    public void Refresh()
    {
        AttachToParent();
    }

    private void AttachToParent()
    {
        if (bulletHoleMesh == null)
            return;

        RaycastHit hit;
        Vector3 rayStart = transform.position;
        Vector3 rayDirection = -transform.forward;

        if (Physics.Raycast(rayStart, rayDirection, out hit, 1.0f))
        {
            transform.SetParent(hit.collider.transform);
        }
    }

    private void FadeAndDieOverTime()
    {
        timer += Time.deltaTime;

        if (timer >= startFadeTime && bulletHoleMesh != null)
        {
            Renderer renderer = bulletHoleMesh.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.Lerp(renderer.material.color, targetColor, fadeRate * Time.deltaTime);
            }
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}