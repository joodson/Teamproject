using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivityController : MonoBehaviour
{
    public Slider slider;
    public settings lookScript; // replace with Rayan script name

    void Start()
    {
        // Set slider default to current sensitivity
        slider.value = lookScript.mouseSensitivity;

        // Listen for changes
        slider.onValueChanged.AddListener(UpdateSensitivity);
    }

    void UpdateSensitivity(float value)
    {
        lookScript.mouseSensitivity = value;
    }
}