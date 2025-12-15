using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalBossHealth : MonoBehaviour
{
    public EnemyHealth enemyHealth;   // نسحب EnemyHealth هنا
    
    [SerializeField] private string winningSceneName = "winning";

    public void TakeDamage(float amount)
    {
        enemyHealth.TakeDamage(amount);

        if (enemyHealth.health <= 0)
        {
             Invoke(nameof(LoadLosingScene), 2f);
             
        }
    }
    
     private void LoadLosingScene()
        
    {
        SceneManager.LoadScene(winningSceneName);
    }   
}