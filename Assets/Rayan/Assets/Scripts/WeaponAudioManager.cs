using UnityEngine;

public class WeaponAudioManager : MonoBehaviour
{
    [Header("Audio Sources")] // Speakers for weapon sounds
    [SerializeField] private AudioSource shootAudioSource;    // For shooting sounds
    [SerializeField] private AudioSource reloadAudioSource;   // For reload sounds
    [SerializeField] private AudioSource emptyAudioSource;    // For empty/click sounds

    [Header("Shooting Clips")]
    [SerializeField] private AudioClip[] shootSounds;         // 2-3 shoot variations
    [SerializeField] private AudioClip[] suppressedShootSounds; // Optional: silenced shots

    [Header("Reload Clips")]
    [SerializeField] private AudioClip[] reloadSounds;        // 1-2 reload sounds
    [SerializeField] private AudioClip magOutSound;           // Magazine removal
    [SerializeField] private AudioClip magInSound;            // Magazine insert
    [SerializeField] private AudioClip boltSound;             // Bolt/slide pull

    [Header("Empty/Dry Fire Clips")]
    [SerializeField] private AudioClip[] emptySounds;         // 1-2 empty click sounds

    [Header("Impact Clips (for bullet hits)")]
    [SerializeField] private AudioClip[] concreteImpactSounds;  // Wall hits
    [SerializeField] private AudioClip[] metalImpactSounds;     // Metal hits
    [SerializeField] private AudioClip[] woodImpactSounds;      // Wood hits
    [SerializeField] private AudioClip[] groundImpactSounds;    // Dirt/ground hits
    [SerializeField] private AudioClip[] fleshImpactSounds;     // Zombie hits (blood splatter)

    [Header("Audio Settings")]
    [SerializeField] private float shootVolume = 1f;
    [SerializeField] private float reloadVolume = 0.8f;
    [SerializeField] private float impactVolume = 0.6f;

    // Singleton pattern (same as PlayerSoundManager)
    public static WeaponAudioManager Instance;

    private void Awake()
    {
        // Set singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Auto-create audio sources if not assigned
        if (shootAudioSource == null)
            shootAudioSource = CreateAudioSource("ShootSource");

        if (reloadAudioSource == null)
            reloadAudioSource = CreateAudioSource("ReloadSource");

        if (emptyAudioSource == null)
            emptyAudioSource = CreateAudioSource("EmptySource");
    }

    // ========== SHOOTING SOUNDS ==========
    public void PlayShootSound(bool isSuppressed = false)
    {
        AudioClip[] soundsToUse = isSuppressed ? suppressedShootSounds : shootSounds;
        PlayRandomClip(shootAudioSource, soundsToUse, shootVolume);
    }

    // ========== RELOAD SOUNDS ==========
    public void PlayReloadSound()
    {
        // Play full reload sound (single long sound)
        PlayRandomClip(reloadAudioSource, reloadSounds, reloadVolume);
    }

    // Advanced: Detailed reload with separate sounds
    public void PlayDetailedReload()
    {
        // This method plays reload sounds in sequence
        // Call this from WeaponController with delays
        StartCoroutine(DetailedReloadSequence());
    }

    private System.Collections.IEnumerator DetailedReloadSequence()
    {
        // Magazine out
        if (magOutSound != null)
        {
            reloadAudioSource.PlayOneShot(magOutSound, reloadVolume);
            yield return new WaitForSeconds(0.5f);
        }

        // Magazine in
        if (magInSound != null)
        {
            reloadAudioSource.PlayOneShot(magInSound, reloadVolume);
            yield return new WaitForSeconds(0.4f);
        }

        // Bolt pull
        if (boltSound != null)
        {
            reloadAudioSource.PlayOneShot(boltSound, reloadVolume);
        }
    }

    // ========== EMPTY/DRY FIRE SOUNDS ==========
    public void PlayEmptySound()
    {
        PlayRandomClip(emptyAudioSource, emptySounds, shootVolume);
    }

    // ========== IMPACT SOUNDS ==========
    public void PlayImpactSound(SurfaceType surfaceType, Vector3 position)
    {
        AudioClip[] soundsToPlay = GetImpactSoundsBySurface(surfaceType);

        if (soundsToPlay != null && soundsToPlay.Length > 0)
        {
            // Play 3D sound at impact position
            AudioClip clip = soundsToPlay[Random.Range(0, soundsToPlay.Length)];
            AudioSource.PlayClipAtPoint(clip, position, impactVolume);
        }
    }

    private AudioClip[] GetImpactSoundsBySurface(SurfaceType surface)
    {
        switch (surface)
        {
            case SurfaceType.Concrete:
                return concreteImpactSounds;
            case SurfaceType.Metal:
                return metalImpactSounds;
            case SurfaceType.Wood:
                return woodImpactSounds;
            case SurfaceType.Ground:
                return groundImpactSounds;
            case SurfaceType.Flesh:
                return fleshImpactSounds;
            default:
                return concreteImpactSounds; // Default to concrete
        }
    }

    // ========== HELPER METHODS ==========
    private void PlayRandomClip(AudioSource source, AudioClip[] clips, float volume = 1f)
    {
        if (clips == null || clips.Length == 0 || source == null)
            return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.PlayOneShot(clip, volume);
    }

    private AudioSource CreateAudioSource(string sourceName)
    {
        GameObject sourceObj = new GameObject(sourceName);
        sourceObj.transform.SetParent(transform);
        sourceObj.transform.localPosition = Vector3.zero;

        AudioSource source = sourceObj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 0.5f; // Mix between 2D and 3D sound
        source.maxDistance = 50f;

        return source;
    }

    // ========== PUBLIC UTILITY METHODS ==========

    // Stop all weapon sounds (useful for weapon switching)
    public void StopAllSounds()
    {
        if (shootAudioSource != null) shootAudioSource.Stop();
        if (reloadAudioSource != null) reloadAudioSource.Stop();
        if (emptyAudioSource != null) emptyAudioSource.Stop();
    }

    // Adjust volumes at runtime
    public void SetShootVolume(float volume)
    {
        shootVolume = Mathf.Clamp01(volume);
    }

    public void SetReloadVolume(float volume)
    {
        reloadVolume = Mathf.Clamp01(volume);
    }

    public void SetImpactVolume(float volume)
    {
        impactVolume = Mathf.Clamp01(volume);
    }
}