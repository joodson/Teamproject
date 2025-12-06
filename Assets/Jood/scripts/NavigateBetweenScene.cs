using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateBetweenScene : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}