using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovemnt player = other.GetComponent<playerMovemnt>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}
