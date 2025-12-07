using UnityEngine;

public class settings : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject SoundControl;
    //this line will be deleted after merging with player script
    public float mouseSensitivity = 2;

    public void Setting()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            SoundControl.SetActive(false);
            isPaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            SoundControl.SetActive(true);
            isPaused = true;
        }
    }
}
