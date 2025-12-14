using TMPro;
using UnityEngine;

public class WinTimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeValueText;
    public TextMeshProUGUI scoreValueText;

    void Start()
    {
        float t = TimerManager.Instance.TimeElapsed;
        timeValueText.text = FormatTime(t);
        scoreValueText.text = ScoreManager.Instance.Score.ToString();
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int ms = Mathf.FloorToInt((t * 1000f) % 1000f);

        return $"{minutes:00}:{seconds:00}.{ms:000}";
    }
}
