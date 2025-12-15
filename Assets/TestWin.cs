using UnityEngine;
using UnityEngine.SceneManagement;

public class TestWin : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("winning");
        }
    }
}
