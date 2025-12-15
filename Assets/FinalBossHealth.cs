using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossHealth : MonoBehaviour
{
    public EnemyHealth enemyHealth;   // نسحب EnemyHealth هنا
    public string winSceneName = "winning";

    public void TakeDamage(float amount)
    {
        enemyHealth.TakeDamage(amount);

        if (enemyHealth.health <= 0)
        {
            SceneManager.LoadScene("winning");
        }
    }
}