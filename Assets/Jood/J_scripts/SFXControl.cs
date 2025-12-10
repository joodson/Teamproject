using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXControl : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider sfxSlider;

    void Start()
    {
        sfxSlider.onValueChanged.AddListener(SetSFX);
    }

    public void SetSFX(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mixer.SetFloat("SFXVolume", dB);
    }
}