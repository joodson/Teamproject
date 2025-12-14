using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeValueText;

    void OnEnable()
    {
        if (timeValueText == null)
            timeValueText = GameObject.Find("TimeValue")?.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (TimerManager.Instance == null || timeValueText == null) return;

        float t = TimerManager.Instance.TimeElapsed;
        timeValueText.text = FormatTime(t);
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}.{ms:000}";
    }
}
