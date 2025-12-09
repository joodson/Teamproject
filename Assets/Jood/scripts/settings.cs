using UnityEngine;

public class settings : MonoBehaviour
{
    public GameObject SoundControl;
    //this line will be deleted after merging with player script
    public float mouseSensitivity = 2;

    public void OpenSetting()
    {
            Time.timeScale = 0f;
            SoundControl.SetActive(true);
    }
    public void CloseSetting()
    {
        Time.timeScale = 1f;
        SoundControl.SetActive(false);
    }
    }
