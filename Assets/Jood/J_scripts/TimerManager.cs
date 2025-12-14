using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    public float TimeElapsed { get; private set; }
    public bool IsRunning { get; private set; }

    [SerializeField] private string gameplaySceneName = "SampleScene"; 
    [SerializeField] private bool resetOnGameplayEnter = true; 

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Update()
    {
        if (IsRunning)
            TimeElapsed += Time.deltaTime;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameplaySceneName)
        {
            if (resetOnGameplayEnter) TimeElapsed = 0f;
            IsRunning = true;
        }
        else
        {
            IsRunning = false;
        }
    }

    public void StartTimer()
    {
        TimeElapsed = 0f;
        IsRunning = true;
    }

    public void StopTimer()
    {
        IsRunning = false;
    }
}
