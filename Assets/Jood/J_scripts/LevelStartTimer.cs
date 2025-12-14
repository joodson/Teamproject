using UnityEngine;

public class LevelStartTimer : MonoBehaviour
{
    void Start()
    {
        if (TimerManager.Instance != null)
            TimerManager.Instance.StartTimer();
    }
}
