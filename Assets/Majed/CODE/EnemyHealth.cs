using UnityEngine;

using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100f;
    private string winningSceneName = "winning";
    bool finalbossdefeated = false;
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health >= 700 || finalbossdefeated)
        {
            finalbossdefeated = true;
            if (health <= 0)
        {
            Die();
            SceneManager.LoadScene(winningSceneName);

        }
        }

        if (health <= 0)
        {
            Die();
        }

    }

    private void Die()
    {
        ScoreManager.Instance.AddScore(10);
        Destroy(gameObject);
    }
}