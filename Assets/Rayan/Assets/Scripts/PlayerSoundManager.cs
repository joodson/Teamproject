using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [Header("Audio Sources")]  // it is like speakers hardware only does not have file sounds
    [SerializeField] private AudioSource footstepAudioSource;  // One speaker for all footsteps
    [SerializeField] private AudioSource actionAudioSource;    // One speaker for jumps/obstacles

    [Header("Footstep Clips (Short sounds)")]
    [SerializeField] private AudioClip[] walkSounds;           // 3-5 short walk sounds
    [SerializeField] private AudioClip[] runSounds;            // 3-5 short run sounds
    [SerializeField] private AudioClip[] crouchSounds;         // 3-5 short crouch sounds

    [Header("Action Clips")]
    [SerializeField] private AudioClip[] jumpSounds;           // 1-2 jump sounds
    [SerializeField] private AudioClip[] landSounds;           // 1-2 landing sounds
    [SerializeField] private AudioClip[] obstacleSounds;       // 1-2 obstacle hit sounds

    [Header("Footstep Timing")]
    [SerializeField] private float walkInterval = 0.5f;
    [SerializeField] private float runInterval = 0.3f;
    [SerializeField] private float crouchInterval = 0.7f;

    // Singleton
    public static PlayerSoundManager Instance;

    private float _footstepTimer;

    void Awake()
    {
            Instance = this;
    }

    // Play footstep based on movement state
    public void PlayFootstep(MovementState state, float deltaTime)
    {
        _footstepTimer -= deltaTime;

        if (_footstepTimer <= 0f)
        {
            AudioClip[] sounds;
            float interval;

            switch (state)
            {
                case MovementState.Running:
                    sounds = runSounds;
                    interval = runInterval;
                    break;
                case MovementState.Crouching:
                    sounds = crouchSounds;
                    interval = crouchInterval;
                    break;
                default:
                    sounds = walkSounds;
                    interval = walkInterval;
                    break;
            }

            PlayRandomClip(footstepAudioSource, sounds);
            _footstepTimer = interval;
        }
    }

    // Reset footstep timer when player stops
    public void ResetFootstepTimer()
    {
        _footstepTimer = 0f;
    }

    public void PlayJumpSound()
    {
        PlayRandomClip(actionAudioSource, jumpSounds);
    }

    public void PlayLandSound()
    {
        PlayRandomClip(actionAudioSource, landSounds);
    }

    public void PlayObstacleSound()
    {
        PlayRandomClip(actionAudioSource, obstacleSounds);
    }

    // Helper: Play a random clip from an array
    private void PlayRandomClip(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length > 0 && source != null)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            source.PlayOneShot(clip); // PlayOneShot = play without interrupting other sounds
        }
    }
}

public enum MovementState
{
    Walking,
    Running,
    Crouching
}