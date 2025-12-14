using UnityEngine;

public class EnsureTimeScale : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
    }
}
