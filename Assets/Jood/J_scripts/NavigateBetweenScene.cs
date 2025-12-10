using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigateBetweenScene : MonoBehaviour
{
    private static string lastScene = null;

    public void LoadSceneByName(string sceneName)
    {
        // Save current scene as "last scene"
        lastScene = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName);
    }

    public void LoadLastScene()
    {
        if (!string.IsNullOrEmpty(lastScene))
        {
            SceneManager.LoadScene(lastScene);
        }
        else
        {
            // fallback if no history
            SceneManager.LoadScene("Jood");  
        }
    }
}
