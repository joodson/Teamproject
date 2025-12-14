using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int Score { get; private set; }

    [SerializeField] private string gameplaySceneName = "SampleScene"; 
    [SerializeField] private bool resetOnGameplayEnter = true;

    void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddScore(10);
        }
    }
    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameplaySceneName && resetOnGameplayEnter)
            Score = 0;
    }

    public void AddScore(int amount)
    {
        Score += amount;
        if (Score < 0) Score = 0;
    }

    public void ResetScore()
    {
        Score = 0;
    }
}
