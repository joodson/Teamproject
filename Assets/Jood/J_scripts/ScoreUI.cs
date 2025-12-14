using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreValueText;

    void OnEnable()
    {
        if (scoreValueText == null)
            scoreValueText = GameObject.Find("ScoreValue")?.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (ScoreManager.Instance == null || scoreValueText == null) return;

        scoreValueText.text = ScoreManager.Instance.Score.ToString();
    }
}
